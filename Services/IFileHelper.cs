namespace MyCarearApi.Services;

public interface IFileHelper
{
    bool FileValidateImage(IFormFile file);
    bool FileValidate(IFormFile file);
    Task<string> WriteFileAsync(IFormFile file, string folder);
    bool DeleteFileByName(string fileName);
}