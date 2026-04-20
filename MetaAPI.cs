using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class MetaAPI
{
    public Dictionary<string, string> GetMeta(string filename)
    {
        var meta = new Dictionary<string, string>();
        try
        {
            FileInfo fileinfo = new FileInfo(filename);
            meta["CreationTime"] = fileinfo.CreationTime.ToString();
            meta["LastWriteTime"] = fileinfo.LastWriteTime.ToString();
            meta["Size"] = fileinfo.Length.ToString();
            meta["Attributes"] = fileinfo.Attributes.ToString();
            return meta;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return meta;
        }
    }
}