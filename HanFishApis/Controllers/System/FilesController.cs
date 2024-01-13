using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace HanFishApis.Controllers.System
{
    public class FilesController : BaseController
    {
        public FilesController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> UploadFilesAsync(IEnumerable<IFormFile> formFiles)
        {
            var result = await FileHelper.UploadFilesAsync(FileHelper.FileType.WebResource, formFiles);
            return JsonSuccess(result, "上传成功");
        }

        [HttpPost]
        public async Task<IActionResult> UploadDefualtFilesAsync(IEnumerable<IFormFile> formFiles)
        {
            var result = await FileHelper.UploadFilesAsync(FileHelper.FileType.SystemResource, formFiles);
            return JsonSuccess(result, "上传成功");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile)
        {
            var result = await FileHelper.UploadFilesAsync(FileHelper.FileType.WebResource, new List<IFormFile>() { formFile } );
            return JsonSuccess(result, "上传成功");
        }

        [HttpPost]
        public async Task<IActionResult> UploadDefualtFileAsync(IFormFile formFile)
        {
            var result = await FileHelper.UploadFilesAsync(FileHelper.FileType.SystemResource, new List<IFormFile>() { formFile });
            return JsonSuccess(result, "上传成功");
        }
    }
}
