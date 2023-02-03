using BaseAPI.Controllers;
using Entities.Catalogue;
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
using Models.Catalogue;
using Request.Catalogue.CatalogueCreate;
using Request.Catalogue.CatalogueUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers.Catalogue
{
    /// <summary>
    /// Quản lý loại kỹ thuật
    /// </summary>
    [Route("api/technicaloption")]
    [ApiController]
    [Description("Quản lý loại kỹ thuật")]
    [Authorize]
    public class TechnicalOptionController : BaseCatalogueController<tbl_TechnicalOptions, TechnicalOptionModel, TechnicalOptionCreate, TechnicalOptionUpdate, TechnicalOptionsSearch>
    {
        protected readonly ITechnicalOptionService technicalOptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ITechnicalProductService technicalProductService;

        public TechnicalOptionController(IServiceProvider serviceProvider, ILogger<BaseCatalogueController<tbl_TechnicalOptions, TechnicalOptionModel, TechnicalOptionCreate, TechnicalOptionUpdate, TechnicalOptionsSearch>> logger
            , IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            technicalProductService = serviceProvider.GetRequiredService<ITechnicalProductService>();
            catalogueService = serviceProvider.GetRequiredService<ITechnicalOptionService>();
            _httpContextAccessor = httpContextAccessor;
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
            //Kiểm tra trong các tiêu chuẩn kỹ thuật có loại kỹ thuật không
            var count = technicalProductService.GetCountTechnicalByOptionId(id).Result;
            if (count > 0)
            {
                throw new KeyNotFoundException("Loại kỹ thuật này đang được sử dụng, không thể xóa!");
            }
            tbl_TechnicalOptions entity = await catalogueService.DeleteDataAsync(id);
            if (entity == null)
                throw new Exception("Lỗi trong quá trình xử lý!");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Xóa dữ liệu thành công!",
                Success = true
            };
        }

    }
}
