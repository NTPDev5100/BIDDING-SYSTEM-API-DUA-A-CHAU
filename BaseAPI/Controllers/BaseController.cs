using AutoMapper;
using Entities.DomainEntities;
using Extensions;
using Interface.Services.DomainServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.DomainModels;
using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Utilities;

namespace BaseAPI.Controllers
{
    [ApiController]
    public abstract class BaseController<E, T, C, U, F> : ControllerBase
        where E : Entities.DomainEntities.DomainEntities
        where T : AppDomainModel
        where C : DomainCreate
        where U : DomainUpdate
        where F : BaseSearch, new()
    {
        protected readonly ILogger<BaseController<E, T, C, U, F>> logger;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IMapper mapper;
        protected IDomainService<E, F> domainService;
        protected IWebHostEnvironment env;

        public BaseController(IServiceProvider serviceProvider, ILogger<BaseController<E, T, C, U, F>> logger, IWebHostEnvironment env)
        {
            this.env = env;
            this.logger = logger;
            this.mapper = serviceProvider.GetService<IMapper>();
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Lấy thông tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AppAuthorize]
        [Description("Xem thông tin chi tiết")]
        public virtual async Task<AppDomainResult> GetById(Guid id)
        {
            E item = await this.domainService.GetByIdAsync(id);
            if (item != null)
            {
                var itemModel = mapper.Map<T>(item);
                return new AppDomainResult() { Success = true, Data = itemModel, ResultCode = (int)HttpStatusCode.OK };
            }
            throw new KeyNotFoundException("Item không tồn tại");
        }

        /// <summary>
        /// Thêm mới item
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới dữ liệu")]
        public virtual async Task<AppDomainResult> AddItem([FromBody] C itemModel)
        {
            if (ModelState.IsValid)
            {
                E item = mapper.Map<E>(itemModel);
                if (item != null)
                {
                    bool success = await this.domainService.CreateAsync(item);
                    if (!success)
                        throw new Exception("Lỗi trong quá trình xử lý");
                    return new AppDomainResult() { Success = true, ResultMessage = "Thêm mới thành công!", ResultCode = (int)HttpStatusCode.OK };
                }
                else
                    throw new AppException("Item không tồn tại");
            }
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
        public virtual async Task<AppDomainResult> UpdateItem([FromBody] U itemModel)
        {
            if (ModelState.IsValid)
            {
                var item = mapper.Map<E>(itemModel);
                if (item != null)
                {
                    var jtem = await this.domainService.GetByIdAsync(item.Id);
                    bool success = await this.domainService.UpdateAsync(item);
                    if (!success)
                        throw new Exception("Lỗi trong quá trình xử lý");
                    return new AppDomainResult() { Success = true, ResultMessage = "Cập nhật thành công!", ResultCode = (int)HttpStatusCode.OK };
                }
                else
                    throw new KeyNotFoundException("Item không tồn tại");
            }
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
        public virtual async Task<AppDomainResult> DeleteItem(Guid id)
        {
            E entity = await this.domainService.DeleteDataAsync(id);
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
                throw new Exception("Lỗi trong quá trình xử lý");
        }

        /// <summary>
        /// Lấy danh sách item phân trang
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Xem danh sách")]
        public virtual async Task<AppDomainResult> Get([FromQuery] F baseSearch)
        {
            if (ModelState.IsValid)
            {
                PagedList<E> pagedData = await this.domainService.GetPagedListData(baseSearch);
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

        [NonAction]
        public List<Permission> ListPermission()
        {
            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            List<Permission> permissions = new List<Permission>();
            foreach (Assembly assem in assems)
            {
                var controller = assem.GetTypes().Where(type =>
                typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                  .Select(e => new Permission()
                  {
                      Controller = e.Name.Replace("Controller", string.Empty),
                      Description = string.Format("{0}", ReflectionUtilities.GetClassDescription(e)).Replace("Controller", string.Empty),
                      PermissionActions = (from i in e.GetMembers().Where(x => (x.Module.Name == "API.dll" || x.Module.Name == "BaseAPI.dll") && x.Name != ".ctor" && x.Name != "GetPermission" && x.Name != "ListPermission")
                                           select new PermissionActions
                                           {
                                               Action = i.Name,
                                               Description = i.GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                           .Cast<DescriptionAttribute>().Select(d => d.Description)
                                                           .SingleOrDefault() ?? i.Name,
                                               Allowed = false
                                           }).ToList(),
                  })
                  .Where(e => e.Controller != "Role" && e.Controller != "Auth" && e.Controller != "Menu" && e.Controller != "Catalogue")
                  .OrderBy(e => e.Controller)
                  .Distinct();

                permissions.AddRange(controller);
            }
            return permissions;
        }

        [NonAction]
        public async Task<Permission> GetPermission(string controllerName)
        {
            return await Task.Run(() =>
            {
                System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();
                Permission permissions = new Permission();

                foreach (Assembly assem in assems)
                {
                    permissions = assem.GetTypes().Where(type =>
                    typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract && type.Name.Replace("Controller", string.Empty) == controllerName)
                      .Select(e => new Permission()
                      {
                          Controller = e.Name.Replace("Controller", string.Empty),
                          Description = string.Format("{0}", ReflectionUtilities.GetClassDescription(e)).Replace("Controller", string.Empty),
                          PermissionActions = (from i in e.GetMembers().Where(x => (x.Module.Name == "API.dll" || x.Module.Name == "BaseAPI.dll") && x.Name != ".ctor")
                                               select new PermissionActions
                                               {
                                                   Action = i.Name,
                                                   Description = i.GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                               .Cast<DescriptionAttribute>().Select(d => d.Description)
                                                               .SingleOrDefault() ?? i.Name,
                                               }).ToList(),
                      }).FirstOrDefault();
                    if (permissions != null)
                        break;
                }
                return permissions;
            });
        }
    }
}