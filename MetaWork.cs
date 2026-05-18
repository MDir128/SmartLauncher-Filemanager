using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TagLib;

public class MetaWork
{
    private static readonly HashSet<string> SystemMetaKeys = new()
    {
        "CreationTime",
        "LastWriteTime",
        "Size",
        "Attributes"
    };

    //системные метаданные CreationTime, LastWriteTime, Size, Attributes не изменяются!!!   
    //множество ключей разрешённых на изменение метаданных
    private static readonly HashSet<string> AccessedMetaKeys = new()
    {
        "Title",
        "Author",
        "Comment",
        "Year"
    };

    //изменение метаданных
    public bool ChangeMeta(FileBox file, string key, string newMeta)
    {
        try
        {
            if (file == null)
            {
                return false;
            }
            if (!AccessedMetaKeys.Contains(key))
            {
                return false;
            }
            if (file.Meta == null)
            {
                file.Meta = new Dictionary<string, string>();
            }
            file.Meta[key] = newMeta;
            return true;
        }
        catch
        {
            return false;
        }
    }

    //изменение самого файла
    public bool UpdateFile(FileBox file)
    {
        try
        {
            if (file == null || string.IsNullOrEmpty(file.Address))
            {
                return false;
            }
            if (file.Meta == null)
            {
                file.Meta = new Dictionary<string, string>();
            }
            if (file.Category == "music" || file.Category == "video")
            {
                using var tagfile = TagLib.File.Create(file.Address);
                if (file.Meta.ContainsKey("Title"))
                {
                    tagfile.Tag.Title = file.Meta["Title"];
                }
                if (file.Meta.ContainsKey("Author"))
                {
                    tagfile.Tag.Performers = new[] { file.Meta["Author"] };
                }
                if (file.Meta.ContainsKey("Comment"))
                {
                    tagfile.Tag.Comment = file.Meta["Comment"];
                }
                if (file.Meta.ContainsKey("Year"))
                {
                    if (uint.TryParse(file.Meta["Year"], out uint year))
                    {
                        tagfile.Tag.Year = year;
                    }
                }
                tagfile.Save();
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}