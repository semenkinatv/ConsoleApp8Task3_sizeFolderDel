using System;
using System.IO;
namespace Modul8Task1
{
    public static class DirExtension
    {  //размер папки со всеми файлами и вложенными папками
        public static long DirSize(string URL)
        {
            DirectoryInfo dir = new DirectoryInfo(URL);
            long size = 0;
            FileInfo[] fis = dir.GetFiles();
            foreach (FileInfo fi in fis)
            { size += fi.Length; }

            DirectoryInfo[] dis = dir.GetDirectories();
            foreach (DirectoryInfo di in dis)
            { size += DirSize(di.FullName); }
            return size;
        }
        public static long FileSize(string URL)
        {
            FileInfo fl = new FileInfo(URL);
            return fl.Length;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {  ///Зададим каталог для очистки
            string delpath = @"/Semenkina/task1"; 
            ClearDirect(delpath);
        }
        
        ///чистит нужную нам папку от файлов  и папок, которые не использовались более 30 минут 
        static void ClearDirect(string Delpath)
        {   string delpath = Delpath;
            long delbyte = 0;
            long delbyteAll = 0;
            try
            {
                if (Directory.Exists(delpath))
                {
                   Console.WriteLine("Зачистка папки {0}", delpath);
                   Console.WriteLine();
                   Console.WriteLine("Исходный размер папки: {0} байт.", DirExtension.DirSize(delpath));

                    Console.WriteLine("Файлы:");
                    string[] files = Directory.GetFiles(delpath);// Получим все файлы корневого каталога

                    foreach (string s in files)   // Выведем их все
                    {
                        /* GetLastAccessTime() в windows не отражает дату последнего использования
                          будем использовать дату последнего изменения файла File.GetLastWriteTime()                        */

                        delbyte = DirExtension.FileSize(s);
                        Console.Write($"Файл {s} - изменен {File.GetLastWriteTime(s)}, размер {delbyte}");

                        if (DateTime.Now - File.GetLastWriteTime(s) > TimeSpan.FromMinutes(30))
                        {
                            delbyteAll += delbyte;
                            File.Delete(s);
                            Console.Write($" - УДАЛЕН.");

                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    Console.WriteLine("Папки:");
                    /// Получим все директории корневого каталога
                    string[] dirs = Directory.GetDirectories(delpath); 
                    

                    foreach (string d in dirs) // Выведем их все
                    {
                        delbyte = DirExtension.DirSize(d);
                        Console.Write($"Папка {d} - изменена {Directory.GetLastWriteTime(d)}, размер  {delbyte} байт");
                        if (DateTime.Now - Directory.GetLastWriteTime(d) > TimeSpan.FromMinutes(30))
                        {
                            delbyteAll += delbyte;
                            Directory.Delete(d, true);

                            Console.Write($"- УДАЛЕНА.");
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine($"Освобождено {delbyteAll} байт") ;
                    Console.WriteLine("Текущий размер папки: {0} байт.", DirExtension.DirSize(delpath));

                }
                else
                {
                    Console.WriteLine($"Очистка невозможна. Папка по заданному адресу ({delpath}) - не существует.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
