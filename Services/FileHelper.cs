#pragma warning disable
using Microsoft.AspNetCore.Http;

namespace MyCarearApi.Services;
public class FileHelper : IFileHelper
{

    // Fileni imagega tekshiradi Imagefilega tekshirish kerak bo'lsa shu funksiyadan fooydalaniylar
    public bool FileValidateImage(IFormFile file)
    {
        var defineFileExtension = DefineFileExtension(file);

        if ((defineFileExtension.ToLower() == "png" || defineFileExtension.ToLower() == "jpg" || defineFileExtension.ToLower() == "jpeg"))
            return true;

        return false;
    }
    //  istalgan filega tekshiradi kimgadur filega tekshirish kerak bo'lsa shu funksiya
    public bool FileValidate(IFormFile file)
    {
        var defineFileExtension = DefineFileExtension(file);

        if (file.Length > 0)
            return true;

        return false;
    }
    // fileni Data ichidagi Folders file ichiga saqlab ketiladi Folders File ichida yana file ochmoqchi bo'lsangiz FileFolder classiga qo'shib qo'ying
    // fileni to'liq pathini qaytaradi
    public async Task<string> WriteFileAsync(IFormFile file, string folder)
    {
        var fileExtension = DefineFileExtension(file);
        var filename = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'hh'-'mm'-'ss") + "." + fileExtension;
        var filePath = Folder(folder) + @"\" + filename;

        using var fileStream = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write);
        await file.CopyToAsync(fileStream);

        return filename;
    }
    // saqlangan file tipini bo'lsa o'chirib tashlaydi bo'lmasa false qaytaradi
    public bool DeleteFileByName(string filePath, string fileName)
    {
        string? path = filePath + @"\" + fileName;

        if (File.Exists(path))
        {
            // If file found, delete it    
            File.Delete(path);

            return true;
        }
        if (!File.Exists(path))
            return true;

        return false;
    }



    // Extention file kengaytmasini qaytaradi
    private string DefineFileExtension(IFormFile file)
    {
        var reverseFileName = Reverse(file.FileName);
        var count = reverseFileName.Count();
        var index = reverseFileName.IndexOf(".");
        reverseFileName = reverseFileName.Substring(0, index);

        return Reverse(reverseFileName);
    }
    //Revers
    private string Reverse(string fileName)
    {

        char[] charArray = fileName.ToCharArray();
        string reversedString = String.Empty;
        int length, index;
        length = charArray.Length - 1;
        index = length;

        while (index > -1)
        {
            reversedString = reversedString + charArray[index];
            index--;
        }

        return reversedString;
    }
    //    file Pathni ko'rsatadi 
    public string Folder(string fileFolder) => Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Folders\" + fileFolder);
}