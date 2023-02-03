using BaseAPI.Controllers;
using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Request.Catalogue.CatalogueCreate;
using Request.Catalogue.CatalogueUpdate;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace API.Controllers.Catalogue
{
    /// <summary>
    /// Quản lý sản phẩm
    /// </summary>
    [Route("api/products")]
    [ApiController]
    [Description("Quản lý sản phẩm")]
    [Authorize]
    public class ProductController : BaseCatalogueController<tbl_Products, ProductModel, ProductsCreate, ProductsUpdate, ProductSearch>
    {
        private readonly IProductsService productsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITechnicalProductService technicalProductService;
        private readonly IBiddingService biddingService;
        private readonly IUserService userService;

        public ProductController(IServiceProvider serviceProvider, ILogger<BaseCatalogueController<tbl_Products, ProductModel, ProductsCreate, ProductsUpdate, ProductSearch>> logger
            , IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            biddingService = serviceProvider.GetRequiredService<IBiddingService>();
            catalogueService = serviceProvider.GetRequiredService<IProductsService>();
            _httpContextAccessor = httpContextAccessor;
            technicalProductService = serviceProvider.GetRequiredService<ITechnicalProductService>();
            userService = serviceProvider.GetRequiredService<IUserService>();
        }

        /// <summary>
        /// Thêm mới item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới dữ liệu")]
        public override async Task<AppDomainResult> AddItem([FromBody] ProductsCreate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Products>(itemModel);
            if (item == null)
                throw new KeyNotFoundException("Item không tồn tại!");
            var messageCheck = await catalogueService.GetExistItemMessage(item);
            if (!string.IsNullOrEmpty(messageCheck))
                throw new AppException(messageCheck);
            //if (!string.IsNullOrEmpty(itemModel.TechnicalValue))
            //{
            //    List<ObjectTechnicalProduct> technicalOfProduct = JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(itemModel.TechnicalValue);
            //    List<ObjectTechnicalProduct> technicalInProduct = new List<ObjectTechnicalProduct>();
            //    foreach (ObjectTechnicalProduct itemTechnical in technicalOfProduct)
            //    {
            //        technicalInProduct.Add(new ObjectTechnicalProduct { FileName = itemTechnical.FileName, Link = itemTechnical.Link });
            //    }
            //    item.TechnicalValue = JsonConvert.SerializeObject(technicalInProduct);
            //}
            bool success = await catalogueService.CreateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý!");
            if (!string.IsNullOrEmpty(itemModel.TechnicalValue))
            {
                TechnicalProductCreate technicalProductCreate = new TechnicalProductCreate()
                {
                    TechnicalValue = itemModel.TechnicalValue,
                    ProductId = item.Id
                };
                await technicalProductService.AddItemTechnicalProduct(technicalProductCreate, LoginContext.Instance.CurrentUser.userId);
            }
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới sản phẩm thành công!",
                Success = true,
                Data = item
            };
        }


        /// <summary>
        /// Cập nhật thông tin item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật dữ liệu")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] ProductsUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Products>(itemModel);
            if (item == null)
                throw new KeyNotFoundException("Item không tồn tại!");
            var jtem = await catalogueService.GetByIdAsync(item.Id);
            if (jtem == null)
                throw new KeyNotFoundException("Item không tồn tại!");
            if (jtem.Code != item.Code && !string.IsNullOrEmpty(item.Code))
            {
                var messageUserCheck = await this.catalogueService.GetExistItemMessage(item);
                if (!string.IsNullOrEmpty(messageUserCheck))
                    throw new AppException(messageUserCheck);
            }
            if (!string.IsNullOrEmpty(itemModel.TechnicalValue))
            {
                var technicalProduct = await technicalProductService.GetAsync(x => x.ProductId == itemModel.Id && x.Deleted == false);
                if (technicalProduct.Any())
                {
                    TechnicalProductUpdate technicalProductUpdate = new TechnicalProductUpdate()
                    {
                        TechnicalValue = itemModel.TechnicalValue,
                        Id = technicalProduct.Select(x => x.Id).FirstOrDefault()
                    };
                    await technicalProductService.UpdateItemTechnicalProduct(technicalProductUpdate, LoginContext.Instance.CurrentUser.userId);
                }
                else
                {
                    TechnicalProductCreate technicalProductCreate = new TechnicalProductCreate()
                    {
                        TechnicalValue = itemModel.TechnicalValue,
                        ProductId = itemModel.Id
                    };
                    await technicalProductService.AddItemTechnicalProduct(technicalProductCreate, LoginContext.Instance.CurrentUser.userId);
                }
            }
            bool success = await catalogueService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý!");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Cập nhật dữ liệu thành công!",
                Success = true,
                Data = item
            };
        }


        /// <summary>
        /// Xóa item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AppAuthorize]
        [Description("Xóa dữ liệu")]
        public override async Task<AppDomainResult> DeleteItem(Guid id)
        {
            //Kiểm tra trong các gói thầu có sản phẩm không
            var count = biddingService.GetCountBiddingByProductId(id).Result;
            if (count > 0)
            {
                throw new KeyNotFoundException("Sản phẩm này đang được sử dụng, không thể xóa!");
            }
            //Xóa các tiêu chuẩn kỹ thuật của sản phẩm nếu có
            await technicalProductService.DeleteTechnicalByProductId(id);
            tbl_Products entity = await catalogueService.DeleteDataAsync(id);
            if (entity == null)
                throw new Exception("Lỗi trong quá trình xử lý!");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Xóa dữ liệu thành công!",
                Success = true
            };
        }


        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Xem danh sách")]
        public override async Task<AppDomainResult> Get([FromQuery] ProductSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_Products> pagedData = await this.catalogueService.GetPagedListData(baseSearch);
            PagedList<ProductModel> pagedDataModel = mapper.Map<PagedList<ProductModel>>(pagedData);
            if (pagedDataModel.Items != null && pagedDataModel.Items.Any())
            {
                foreach (var item in pagedDataModel.Items)
                {
                    var user = await userService.GetByIdAsync(item.CreatedBy.Value);
                    if (user != null)
                        item.CreatedName = user.FullName;
                    var technicalProduct = await technicalProductService.GetAsync(x => x.ProductId == item.Id && x.Deleted == false);
                    if (technicalProduct != null && technicalProduct.Any())
                    {
                        item.TechnicalValue = technicalProduct.Where(x => !string.IsNullOrEmpty(x.TechnicalValue)).Select(i => i.TechnicalValue).FirstOrDefault();
                    }
                }
            }
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }

    }
}
