using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class MetaAPI
{
    public Dictionary<string, string> GetMeta(string fileaddress)
    {
        var meta = new Dictionary<string, string>();
        try
        {
            FileInfo fileinfo = new FileInfo(fileaddress);
            //системные мета
            meta["CreationTime"] = fileinfo.CreationTime.ToString();
            meta["LastWriteTime"] = fileinfo.LastWriteTime.ToString();
            meta["Size"] = fileinfo.Length.ToString();
            meta["Attributes"] = fileinfo.Attributes.ToString();
            //другие мета
            meta["Author"] = ""; //имя автора или компании
            meta["Image"] = ""; //путь к изображению
            meta["Comment"] = ""; //примечание или отзыв
            meta["Rating"] = ""; //поставленная оценка
            return meta;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return meta;
        }
    }
}