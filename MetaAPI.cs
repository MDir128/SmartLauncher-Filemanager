using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics; //для метаданных приложений
using TagLib; //для метаданных видео и музыки
using Microsoft.WindowsAPICodePack.Shell; //для метаданных-рейтинга (ПОКА ОСТАВЛЮ, НА ВСЯКИЙ)
using Microsoft.WindowsAPICodePack.Shell.PropertySystem; //для метаданных-рейтинга (ПОКА ОСТАВЛЮ, НА ВСЯКИЙ)


public class MetaAPI
{
    public Dictionary<string, string> GetMeta(string fileaddress)
    {
        var meta = new Dictionary<string, string>();
        try
        {
            FileInfo fileinfo = new FileInfo(fileaddress);
            string fileext = Path.GetExtension(fileaddress).ToString();
            string? filecat = FileCategory.GetCategory(fileext);
            //системные мета
            meta["CreationTime"] = fileinfo.CreationTime.ToString();
            meta["LastWriteTime"] = fileinfo.LastWriteTime.ToString();
            meta["Size"] = $"{fileinfo.Length / 1024.0 / 1024.0:F2} Mb"; //показывает размер в Мб
            meta["Attributes"] = fileinfo.Attributes.ToString();
            //другие мета
            meta["Title"] = ""; //название
            meta["Author"] = ""; //имя автора или компании
            meta["Comment"] = ""; //примечание или отзыв
            //meta["Rating"] = ""; //поставленная оценка
            meta["Duration"] = ""; //длительность
            meta["Year"] = ""; //год релиза

            //ДЛЯ МУЗЫКИ И ВИДЕО
            if (filecat == "music" || filecat == "video")
            {
                var tagfile = TagLib.File.Create(fileaddress);
                meta["Title"] = tagfile.Tag.Title ?? "";
                meta["Author"] = tagfile.Tag.Performers.Length > 0 ? tagfile.Tag.Performers[0] : ""; //возвращает только первого 
                meta["Comment"] = tagfile.Tag.Comment ?? "";
                meta["Duration"] = tagfile.Properties.Duration.ToString() ?? "";
                meta["Year"] = tagfile.Tag.Year > 0 ? tagfile.Tag.Year.ToString() : "";
            }

            //ДЛЯ ПРИЛОЖЕНИЙ
            if (filecat == "game")
            {
                var gameinfo = FileVersionInfo.GetVersionInfo(fileaddress);
                meta["Title"] = gameinfo.FileDescription ?? "";
                meta["Author"] = !string.IsNullOrEmpty(gameinfo.CompanyName) ? gameinfo.CompanyName : (gameinfo.LegalCopyright ?? "");
                meta["Comment"] = gameinfo.ProductName ?? "";
            }

            return meta;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return meta;
        }
    }
}