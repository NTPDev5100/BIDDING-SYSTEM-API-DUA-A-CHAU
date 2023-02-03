using Entities;
using Extensions;
using Interface.Services;
using Interface.Services.Auth;
using Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Request;
using Entities.Search;
using Request.DomainRequests;
using AutoMapper;
using Request.RequestCreate;
using Request.RequestUpdate;
using System.Reflection;
using Models.DomainModels;
using Newtonsoft.Json;
using Entities.DomainEntities;

namespace API.Controllers
{
    [Route("api/address")]
    [ApiController]
    [Description("Thông tin địa chỉ")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        protected ICitiesService citiesService;
        protected IDistrictsService districtsService;
        protected IWardsService wardsService;
        public AddressController(
            ICitiesService citiesService,
            IDistrictsService districtsService,
            IWardsService wardsService)
        {
            this.districtsService = districtsService;
            this.citiesService = citiesService;
            this.wardsService = wardsService;
        }
        /// <summary>
        /// Lấy danh sách tỉnh/thành phố
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("get-cities")]
        [AppAuthorize]
        public virtual async Task<AppDomainResult> GetCities([FromQuery] BaseSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (ModelState.IsValid)
            {
                PagedList<tbl_Cities> pagedData = await citiesService.GetPagedListData(baseSearch);
                var citiesModels = (from i in pagedData.Items
                                    select new CitiesModel
                                    {
                                        Id = i.Id,
                                        Name = i.Name,
                                        Code = i.Code,
                                        Description = i.Description,
                                        Active = i.Active,
                                        Created = i.Created,
                                        CreatedBy = i.CreatedBy,
                                        Updated = i.Updated,
                                        UpdatedBy = i.UpdatedBy,
                                    }).ToList();
                PagedList<CitiesModel> pagedDataModel = new PagedList<CitiesModel>
                {
                    PageIndex = pagedData.PageIndex,
                    PageSize = pagedData.PageSize,
                    TotalItem = pagedData.TotalItem,
                    Items = citiesModels
                };
                appDomainResult = new AppDomainResult
                {
                    Data = pagedDataModel,
                    Success = true,
                    ResultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sách quận/huyện
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("get-districts")]
        [AppAuthorize]
        public virtual async Task<AppDomainResult> GetDistricts([FromQuery] DistrictsSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            if (ModelState.IsValid)
            {
                PagedList<tbl_Districts> pagedData = await districtsService.GetPagedListData(baseSearch);
                var districtsModels = (from i in pagedData.Items.Where(x => x.CityId == baseSearch.CityId || baseSearch.CityId == null)
                                       select new DistrictsModel
                                       {
                                           Id = i.Id,
                                           Name = i.Name,
                                           Code = i.Code,
                                           Description = i.Description,
                                           CityId = i.CityId,
                                           CityName = i.CityName,
                                           Active = i.Active,
                                           Created = i.Created,
                                           CreatedBy = i.CreatedBy,
                                           Updated = i.Updated,
                                           UpdatedBy = i.UpdatedBy,
                                       }).ToList();
                PagedList<DistrictsModel> pagedDataModel = new PagedList<DistrictsModel>
                {
                    PageIndex = pagedData.PageIndex,
                    PageSize = pagedData.PageSize,
                    TotalItem = pagedData.TotalItem,
                    Items = districtsModels
                };
                appDomainResult = new AppDomainResult
                {
                    Data = pagedDataModel,
                    Success = true,
                    ResultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sách phường/xã
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("get-wards")]
        [AppAuthorize]
        public virtual async Task<AppDomainResult> GetWards([FromQuery] WardsSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            if (ModelState.IsValid)
            {
                PagedList<tbl_Wards> pagedData = await wardsService.GetPagedListData(baseSearch);
                var districtsModels = (from i in pagedData.Items.Where(x => x.DistrictId == baseSearch.DistrictId || baseSearch.DistrictId == null)
                                       select new WardsModel
                                       {
                                           Id = i.Id,
                                           Name = i.Name,
                                           Code = i.Code,
                                           Description = i.Description,
                                           DistrictId = i.DistrictId,
                                           DistrictName = i.DistrictName,
                                           Active = i.Active,
                                           Created = i.Created,
                                           CreatedBy = i.CreatedBy,
                                           Updated = i.Updated,
                                           UpdatedBy = i.UpdatedBy,
                                       }).ToList();
                PagedList<WardsModel> pagedDataModel = new PagedList<WardsModel>
                {
                    PageIndex = pagedData.PageIndex,
                    PageSize = pagedData.PageSize,
                    TotalItem = pagedData.TotalItem,
                    Items = districtsModels
                };
                appDomainResult = new AppDomainResult
                {
                    Data = pagedDataModel,
                    Success = true,
                    ResultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
    }
}
