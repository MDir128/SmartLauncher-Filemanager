using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//определение категории объекта по расширению
public class FileCategory
{
    private static readonly string[] GameExtensions = { ".exe" };
    private static readonly string[] VideoExtensions = { ".mkv", ".avi", ".mp4" };
    private static readonly string[] MusicExtensions = { ".mp3", ".wav" };
    public static string? GetCategory(string file_extension)
    {
        file_extension = file_extension.ToLower();
        if (GameExtensions.Contains(file_extension))
        {
            return "game";
        }
        if (VideoExtensions.Contains(file_extension))
        {
            return "video";
        }
        if (MusicExtensions.Contains(file_extension))
        {
            return "music";
        }
        return null;
    }
}