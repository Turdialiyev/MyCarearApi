namespace MyCarearApi.Services;

public interface IFileHelper
{
    bool FileValidateImage(IFormFile file);
    bool FileValidate(IFormFile file);
    Task<string> WriteFileAsync(IFormFile file, string folder);
    string GetFileByName(string fileName);
    string UpdateFileBy(string fileName, IFormFile file);
    bool DeleteFileByName(string fileName);
}