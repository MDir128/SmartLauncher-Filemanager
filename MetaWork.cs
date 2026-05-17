using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

public class MetaWork
{
    //системные метаданные CreationTime, LastWriteTime, Size, Attributes не изменяются!!!

    //множество ключей разрешённых на изменение метаданных
    private static readonly HashSet<string> AccessedMetaKeys = new()
    {
        "Author",
        "Image",
        "Comment",
        "Rating"
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
}