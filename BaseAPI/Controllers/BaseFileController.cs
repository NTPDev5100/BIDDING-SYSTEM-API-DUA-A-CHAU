using Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace BaseAPI.Controllers
{
    [ApiController]
    public abstract class BaseFileController : ControllerBase
    {
        protected readonly ILogger<ControllerBase> logger;
        protected readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseFileController(IServiceProvider serviceProvider, ILogger<ControllerBase> logger, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.env = env;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Upload Single File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload-file")]
        [AppAuthorize]
        [Description("Upload Single File lên hệ thống")]
        public virtual async Task<AppDomainResult> UploadFile(IFormFile file)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            await Task.Run(() =>
            {
                if (file != null && file.Length > 0)
                {
                    string host = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                    string fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), file.FileName);
                    string ext = Path.GetExtension(fileName).ToLower();
                    //if (file.Length > 26214400)
                    //    throw new AppException("Dung lượng file không được lớn hơn 25MB");
                    //if (ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".doc" || ext == ".docx" ||
                    //            ext == ".pdf" || ext == ".xls" || ext == ".xlsm" || ext == ".xlsx" || ext == ".txt")
                    //{
                        string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME);
                        string path = Path.Combine(fileUploadPath, fileName);
                        FileUtilities.CreateDirectory(fileUploadPath);
                        var fileByte = FileUtilities.StreamToByte(file.OpenReadStream());
                        FileUtilities.SaveToPath(path, fileByte);
                        string fileImgPath = host + "/" + fileName;
                        var data = new ObjectFile { Link = fileImgPath, FileName = file.FileName, FileNameSaveInSystem = fileName };
                        appDomainResult = new AppDomainResult()
                        {
                            Success = true,
                            Data = data,
                            ResultMessage = "Upload file thành công!",
                            ResultCode = (int)HttpStatusCode.OK
                        };
                    //}
                    //else
                    //{
                    //    throw new AppException("Upload file sai định dạng!");
                    //}

                }
            });
            return appDomainResult;
        }

        /// <summary>
        /// Upload Multiple File
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("upload-multiple-files")]
        [AppAuthorize]
        [Description("Upload Multiple File lên hệ thống")]
        public virtual async Task<AppDomainResult> UploadFiles(List<IFormFile> files)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            await Task.Run(() =>
            {
                if (files != null && files.Any())
                {
                    foreach (var checkFile in files)
                    {
                        string ext = Path.GetExtension(checkFile.FileName).ToLower();
                        bool success = ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".doc" || ext == ".docx" ||
                                    ext == ".pdf" || ext == ".xls" || ext == ".xlsm" || ext == ".xlsx" || ext == ".txt";
                        if (success == false)
                            throw new AppException($"{checkFile.FileName} định dạnh sai!");
                    }
                    List<string> fileNames = new List<string>();
                    string host = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                    foreach (var file in files)
                    {
                        string fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), file.FileName);
                        string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME);
                        string path = Path.Combine(fileUploadPath, fileName);
                        FileUtilities.CreateDirectory(fileUploadPath);
                        var fileByte = FileUtilities.StreamToByte(file.OpenReadStream());
                        FileUtilities.SaveToPath(path, fileByte);
                        string fileImgPath = host + "/" + fileName;
                        fileNames.Add(fileImgPath);
                    }
                    appDomainResult = new AppDomainResult()
                    {
                        Success = true,
                        Data = fileNames,
                        ResultMessage = "Upload danh sách file thành công!",
                        ResultCode = (int)HttpStatusCode.OK
                    };
                }
            });
            return appDomainResult;
        }


        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="fileNameSaveInSystem"></param>
        /// <returns></returns>
        [HttpDelete("delete-files")]
        [AppAuthorize]
        [Description("Delete File trên hệ thống")]
        public virtual async Task<AppDomainResult> DeleteFile(string fileNameSaveInSystem)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(fileNameSaveInSystem))
                {
                    string fileUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME);
                    string path = Path.Combine(fileUploadPath, fileNameSaveInSystem);
                    FileUtilities.DeleteToPath(path);
                    appDomainResult = new AppDomainResult()
                    {
                        Success = true,
                        ResultMessage = "Delete file thành công!",
                        ResultCode = (int)HttpStatusCode.OK
                    };
                }
            });
            return appDomainResult;
        }
    }
}