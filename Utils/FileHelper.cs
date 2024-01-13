using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Utils
{
    public static class FileHelper
    {
        public enum FileType
        {
            UserProfile,
            SystemResource,
            WebResource
        }

        public static long FileSizeLimit = 52428800; //50 MB

        public static List<string> SuffixAllows = new List<string>() { ".jpg", ".jpeg", ".png" };

        public static async Task<IList<string>> UploadFilesAsync(FileType fileType, IEnumerable<IFormFile> formFiles)
        {
            try
            {
                List<string> files = new List<string>();
                foreach (IFormFile formFile in formFiles)
                {
                    if (formFile.Length > FileSizeLimit)
                        throw new CustomException("文件大小超出限制");
                    var suffix = Path.GetExtension(formFile.FileName);
                    if(!SuffixAllows.Any(s => s.Equals(suffix)))
                        throw new CustomException("不支持上传该文件类型");
                    var fileNmae = formFile.FileName;
                    (var fullPath,var RelativePath) = GetFileName(fileType, suffix);
                    if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    }
                    using FileStream fileStream = File.Create(fullPath);
                    await formFile.CopyToAsync(fileStream);
                    files.Add(RelativePath);
                }
                return files;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        } 

        public static (string, string) GetFileName(FileType fileType, string suffix)
        {
            string saveFilePath = $"Files/{fileType}/{DateTime.Now.ToString("yyyyMMdd")}/{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid().ToString().ToUpper()}{suffix}";
            string allFilePaht = $"{Directory.GetCurrentDirectory()}/{saveFilePath}";
            return (allFilePaht, saveFilePath);
        }
    }
}
