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
    //АВТОМАТИЧЕСКИЙ РЕКУРСИВНЫЙ xПОИСК ПО ЗАДАННОМУ АДРЕСУ
    public List<FileBox> SearchFiles(string file_address)
    {
        var files_list = new List<FileBox>();
        //обработка для безопасности
        try
        {
            FileFound(file_address, files_list);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return files_list;
    }

    //рекурсивный поиск категорий и файлов
    private void FileFound(string address, List<FileBox> files_list)
    {
        //исключения если: папка удалена или нет доступа (системная)
        try
        {
            foreach (var directory in Directory.GetDirectories(address)) //проход по подкаталогам корневой директории
            {
                FileFound(directory, files_list);
            }
            foreach (var file in Directory.GetFiles(address))
            {
                string file_extension = Path.GetExtension(file).ToLower(); //расширение
                string? file_category = FileCategory.GetCategory(file_extension); //категория по расширению
                if (file_category != null)
                {
                    files_list.Add(new FileBox
                    {
                        Address = file,
                        Name = Path.GetFileNameWithoutExtension(file),
                        Extension = file_extension,
                        Category = file_category
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}