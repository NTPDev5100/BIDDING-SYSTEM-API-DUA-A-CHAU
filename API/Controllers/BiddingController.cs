using BaseAPI.Controllers;
using Entities;
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
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý gói thầu
    /// </summary>
    [Route("api/biddings")]
    [ApiController]
    [Description("Quản lý gói thầu")]
    [Authorize]
    public class BiddingController : BaseController<tbl_Biddings, BiddingModel, BiddingsCreate, BiddingsUpdate, BiddingsSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsService productsService;
        private readonly IBiddingSessionService biddingSessionService;
        private readonly IBiddingService biddingService;
        public BiddingController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_Biddings, BiddingModel, BiddingsCreate, BiddingsUpdate, BiddingsSearch>> logger
            , IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            biddingSessionService = serviceProvider.GetRequiredService<IBiddingSessionService>();
            productsService = serviceProvider.GetRequiredService<IProductsService>();
            domainService = serviceProvider.GetRequiredService<IBiddingService>();
            biddingService = serviceProvider.GetRequiredService<IBiddingService>();
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Thêm mới gói thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới gói thầu")]
        public override async Task<AppDomainResult> AddItem([FromBody] BiddingsCreate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Biddings>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại!");
            if (string.IsNullOrEmpty(itemModel.Product))
            {
                return new AppDomainResult() { ResultCode = (int)HttpStatusCode.BadRequest, ResultMessage = "Vui lòng chọn sản phẩm!", Success = false };
            }
            List<ObjectJsonCustom> productOfBidding = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(itemModel.Product);
            foreach (ObjectJsonCustom itemProduct in productOfBidding)
            {
                var product = await productsService.GetByIdAsync(itemProduct.Id);
                if (product == null)
                    throw new AppException("Sản phẩm không tồn tại!");
                item.ProductId = product.Id;
                string code = "ACP_" + product.Code + "_" + Timestamp.DateTimeFormatISO(DateTime.Now);
                item.Code = code;
            }
            bool success = await this.domainService.CreateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới gói thầu thành công!",
                Success = true
            };
        }
        /// <summary>
        /// Cập nhật thông tin gói thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật gói thầu")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] BiddingsUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Biddings>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại!");
            if (string.IsNullOrEmpty(itemModel.Product))
            {
                return new AppDomainResult() { ResultCode = (int)HttpStatusCode.BadRequest, ResultMessage = "Vui lòng chọn sản phẩm!", Success = false };
            }
            List<ObjectJsonCustom> productOfBidding = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(itemModel.Product);
            foreach (ObjectJsonCustom itemProduct in productOfBidding)
            {
                var product = await productsService.GetByIdAsync(itemProduct.Id);
                if (product == null)
                    throw new AppException("Sản phẩm không tồn tại!");
                item.ProductId = product.Id;
            }
            bool success = await this.domainService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult() { ResultCode = (int)HttpStatusCode.OK, ResultMessage = "Cập nhật gói thầu thành công!", Success = true };
        }
        /// <summary>
        /// Danh sách gói thầu
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách gói thầu")]
        public override async Task<AppDomainResult> Get([FromQuery] BiddingsSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_Biddings> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<BiddingModel> pagedDataModel = mapper.Map<PagedList<BiddingModel>>(pagedData);
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }     
    }
}
