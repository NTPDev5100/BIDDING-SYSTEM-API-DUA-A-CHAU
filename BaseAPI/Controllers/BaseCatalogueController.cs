using Entities.DomainEntities;
using Extensions;
using Interface.Services.DomainServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DomainModels;
using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace BaseAPI.Controllers
{
    [ApiController]
    public abstract class BaseCatalogueController<E, T, C, U, F> : BaseController<E, T, C, U, F> where E : AppDomainCatalogue where T : AppDomainCatalogueModel where C : RequestCatalogueCreateModel where U : RequestCatalogueUpdateModel where F : BaseSearch, new()
    {
        protected ICatalogueService<E, F> catalogueService;

        public BaseCatalogueController(IServiceProvider serviceProvider, ILogger<BaseController<E, T, C, U, F>> logger, IWebHostEnvironment env) : base(serviceProvider, logger, env)
        {
        }

        /// <summary>
        /// Lấy thông tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AppAuthorize]
        [Description("Xem thông tin chi tiết")]
        public override async Task<AppDomainResult> GetById(Guid id)
        {
            var item = await this.catalogueService.GetByIdAsync(id);
            if (item != null)
            {
                var itemModel = mapper.Map<T>(item);
                return new AppDomainResult()
                {
                    Success = true,
                    Data = itemModel,
                    ResultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new KeyNotFoundException("Item không tồn tại!");
        }

        /// <summary>
        /// Thêm mới item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới dữ liệu")]
        public override async Task<AppDomainResult> AddItem([FromBody] C itemModel)
        {
            if (ModelState.IsValid)
            {
                var item = mapper.Map<E>(itemModel);
                if (item != null)
                {
                    // Kiểm tra item có tồn tại chưa?
                    var messageUserCheck = await this.catalogueService.GetExistItemMessage(item);
                    if (!string.IsNullOrEmpty(messageUserCheck))
                        throw new AppException(messageUserCheck);
                    bool success = await this.catalogueService.CreateAsync(item);
                    if (!success)
                        throw new Exception("Lỗi trong quá trình xử lý!");
                    return new AppDomainResult()
                    {
                        ResultCode = (int)HttpStatusCode.OK,
                        ResultMessage = "Thêm mới dữ liệu thành công!",
                        Success = true
                    };
                }
                else
                    throw new KeyNotFoundException("Item không tồn tại!");
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
        }

        /// <summary>
        /// Cập nhật thông tin item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật dữ liệu")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] U itemModel)
        {
            if (ModelState.IsValid)
            {
                var item = mapper.Map<E>(itemModel);
                if (item != null)
                {
                    // Kiểm tra item có tồn tại chưa?
                    var jtem = await this.catalogueService.GetByIdAsync(item.Id);
                    if (jtem == null)
                        throw new KeyNotFoundException("Item không tồn tại!");
                    if (jtem.Code != item.Code && !string.IsNullOrEmpty(item.Code))
                    {
                        var messageUserCheck = await this.catalogueService.GetExistItemMessage(item);
                        if (!string.IsNullOrEmpty(messageUserCheck))
                            throw new AppException(messageUserCheck);
                    }
                    bool success = await this.catalogueService.UpdateAsync(item);
                    if (!success)
                        throw new Exception("Lỗi trong quá trình xử lý!");
                    return new AppDomainResult()
                    {
                        ResultCode = (int)HttpStatusCode.OK,
                        ResultMessage = "Cập nhật dữ liệu thành công!",
                        Success = true
                    };
                }
                else
                    throw new KeyNotFoundException("Item không tồn tại");
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
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
            E entity = await this.catalogueService.DeleteDataAsync(id);
            if (entity != null)
            {
                return new AppDomainResult()
                {
                    ResultCode = (int)HttpStatusCode.OK,
                    ResultMessage = "Xóa dữ liệu thành công!",
                    Success = true
                };
            }
            else
                throw new Exception("Lỗi trong quá trình xử lý!");
        }

        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Xem danh sách")]
        public override async Task<AppDomainResult> Get([FromQuery] F baseSearch)
        {
            if (ModelState.IsValid)
            {
                PagedList<E> pagedData = await this.catalogueService.GetPagedListData(baseSearch);
                PagedList<T> pagedDataModel = mapper.Map<PagedList<T>>(pagedData);
                return new AppDomainResult
                {
                    Data = pagedDataModel,
                    Success = true,
                    ResultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
        }
    }
}