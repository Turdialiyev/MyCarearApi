namespace MyCarearApi.Services;

public interface IFileHelper
{
    string Folder(string fileFolder);
    bool FileValidateImage(IFormFile file);
    bool FileValidate(IFormFile file);
    Task<string> WriteFileAsync(IFormFile file, string folder);
    bool DeleteFileByName(string filePath, string fileName);
}