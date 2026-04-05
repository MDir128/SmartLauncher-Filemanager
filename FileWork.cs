using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//получение содержимого файда
public class FileWork
{
    //точка входа для класса
    //АВТОМАТИЧЕСКИЙ РЕКУРСИВНЫЙ ПОИСК ПО ЗАДАННОМУ АДРЕСУ
    public List<string> SearchFiles(string file_address)
    {
        var addresses_list = new List<string>();
        //обработка для безопасности
        try
        {
            FileFound(file_address, addresses_list);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return addresses_list;
    }

    //рекурсивный поиск категорий и файлов
    //ИЩЕТ АДРЕСА
    private void FileFound(string address, List<string> addresses_list)
    {
        //исключения если: папка удалена или нет доступа (системная)
        try
        {
            foreach (var directory in Directory.GetDirectories(address)) //проход по подкаталогам корневой директории
            {
                FileFound(directory, addresses_list);
            }
            foreach (var file in Directory.GetFiles(address))
            {
                string file_extension = Path.GetExtension(file).ToLower(); //расширение
                string? file_category = FileCategory.GetCategory(file_extension); //категория по расширению
                if (file_category != null)
                {
                    addresses_list.Add(file);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //ПРЕВРАЩАЕТ АДРЕСА В FileBox
    public FileBox GetFileData(string file_address)
    {
        string file_extension = Path.GetExtension(file_address).ToLower(); 
        string? file_category = FileCategory.GetCategory(file_extension);
        return new FileBox()
        {
            Address = file_address,
            Name = Path.GetFileNameWithoutExtension(file_address),
            Extension = file_extension,
            Category = file_category,
        };
    }
}