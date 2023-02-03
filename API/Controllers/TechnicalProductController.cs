using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Interface.Services.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý kỹ thuật sản phẩm
    /// </summary>
    [Route("api/technicalproduct")]
    [ApiController]
    [Description("Quản lý kỹ thuật sản phẩm")]
    [Authorize]
    public class TechnicalProductController : BaseController<tbl_TechnicalProduct, TechnicalProductModel, TechnicalProductCreate, TechnicalProductUpdate, TechnicalProductSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ITechnicalOptionService technicalOptionService;

        public TechnicalProductController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_TechnicalProduct, TechnicalProductModel, TechnicalProductCreate, TechnicalProductUpdate, TechnicalProductSearch>> logger
            , IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            technicalOptionService = serviceProvider.GetRequiredService<ITechnicalOptionService>();
            domainService = serviceProvider.GetRequiredService<ITechnicalProductService>();
            this._httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Thêm mới kỹ thuật sản phẩm
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới kỹ thuật sản phẩm")]
        public override async Task<AppDomainResult> AddItem([FromBody] TechnicalProductCreate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_TechnicalProduct>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại!");
            Expression<Func<tbl_TechnicalProduct, tbl_TechnicalProduct>> includeProperties = e => new tbl_TechnicalProduct() { ProductId = e.ProductId };
            IList<tbl_TechnicalProduct> technicalProducts = await domainService.GetAsync(new Expression<Func<tbl_TechnicalProduct, bool>>[] { }, includeProperties);
            if (technicalProducts.Any(x => x.ProductId == item.ProductId))
                throw new AppException("Kỹ thuật của sản phẩm này đã tồn tại!");
            if(string.IsNullOrEmpty(itemModel.TechnicalValue))
            {
                return new AppDomainResult() { ResultCode = (int)HttpStatusCode.BadRequest, ResultMessage = "Vui lòng nhập kỹ thuật sản phẩm!", Success = false };
            }
            List<ObjectTechnicalProduct> technicalOfProduct = JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(itemModel.TechnicalValue);
            List<ObjectTechnicalProduct> technicalInProduct = new List<ObjectTechnicalProduct>();
            foreach(ObjectTechnicalProduct itemTechnical in technicalOfProduct)
            {
                technicalInProduct.Add(new ObjectTechnicalProduct { FileName = itemTechnical.FileName, Link = itemTechnical.Link});
            }
            item.TechnicalValue = JsonConvert.SerializeObject(technicalInProduct);
            bool success = await this.domainService.CreateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới kỹ thuật thành công!",
                Success = true
            };
        }



        /// <summary>
        /// Cập nhật thông tin kỹ thuật sản phẩm
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật kỹ thuật sản phẩm")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] TechnicalProductUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_TechnicalProduct>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại!");
            if (string.IsNullOrEmpty(itemModel.TechnicalValue))
            {
                return new AppDomainResult() { ResultCode = (int)HttpStatusCode.BadRequest, ResultMessage = "Vui lòng nhập kỹ thuật sản phẩm!", Success = false };
            }
            List<ObjectTechnicalProduct> technicalOfProduct = JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(itemModel.TechnicalValue);
            List<ObjectTechnicalProduct> technicalInProduct = new List<ObjectTechnicalProduct>();
            foreach (ObjectTechnicalProduct itemTechnical in technicalOfProduct)
            {
                technicalInProduct.Add(new ObjectTechnicalProduct { FileName = itemTechnical.FileName, Link = itemTechnical.Link });
            }
            item.TechnicalValue = JsonConvert.SerializeObject(technicalInProduct);
            bool success = await this.domainService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult() { ResultCode = (int)HttpStatusCode.OK, ResultMessage = "Cập nhật kỹ thuật sản phẩm thành công!", Success = true };
        }



        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Xem danh sách")]
        public override async Task<AppDomainResult> Get([FromQuery] TechnicalProductSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_TechnicalProduct> pagedData = await domainService.GetPagedListData(baseSearch);
            PagedList<TechnicalProductModel> pagedDataModel = mapper.Map<PagedList<TechnicalProductModel>>(pagedData);         
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }


    }
}
