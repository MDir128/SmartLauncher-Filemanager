using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Hashing;

public class HashAPI
{
    //генерация хэша
    public string HashByDataGen(string filename)
    {
        try
        {
            using var curr_file = File.OpenRead(filename);
            var hash_exemplar = new XxHash3(); //чтобы не загружать весь файл сразу в память, а постепенно через буфер
            byte[] buffer = new byte[1024 * 1024]; 
            int file_bytes; //количество байт из файла, прочитанных за 1 итерацию
            while ((file_bytes = curr_file.Read(buffer, 0, buffer.Length)) > 0)
            {
                hash_exemplar.Append(buffer.AsSpan(0, file_bytes)); //добавление не всего буфера, а только части с заполненными байтами
            }
            byte[] curr_file_hash = hash_exemplar.GetCurrentHash();
            string hash = Convert.ToHexString(curr_file_hash);
            return hash;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }
}