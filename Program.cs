using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleTask
{
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        static void Main(string[] args)
        {
            var _path = Environment.CurrentDirectory + "\\Input\\";

            List<ReadedConfigFile> list = new List<ReadedConfigFile>();

            foreach (string pathfull in Directory.EnumerateFiles(_path, "*.txt",
                SearchOption.TopDirectoryOnly))
            {

                ReadedConfigFile readedConfigFile = new ReadedConfigFile();
                readedConfigFile.FullPath = pathfull;
                readedConfigFile.FileName = Path.GetFileName(pathfull);
                readedConfigFile.AllPackages = ReadFile(pathfull);
                list.Add(readedConfigFile);

            }
            var pathOutPut = Environment.CurrentDirectory + "\\OutputMy\\";

            if (!Directory.Exists(pathOutPut))
                Directory.CreateDirectory(pathOutPut);

            foreach (var pi in list)
            {
                var check = pi.IsInputValidCheckDependencies();

                var newFileName = pi.FileName.Replace("input", "output");
                var fullpath = pathOutPut + newFileName;
                string text = "FAIL";
                if (check)
                    text = "PASS";

                CreateFile(fullpath, text);
            }

        }

        /// <summary>
        /// CheckFile
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public static List<PackagesInfo> ReadFile(string fileName)
        {
            var lines = File.ReadLines(fileName);
            bool isDigit = false;
            List<PackagesInfo> listOfPackagesToInstallInThisConfigFile =
                new List<PackagesInfo>();

            PackagesInfo packagesToInstall = null;

            int helperNumberToCheckHowManyLinesIExecutedAboutDepenedencies = 0;

            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    isDigit = Char.IsDigit(line[0]);
                    if (isDigit)
                    {
                        packagesToInstall = new PackagesInfo();
                        packagesToInstall.NumberPackagesToInstall = int.Parse(line[0].ToString());
                        continue;
                    }

                    if (packagesToInstall != null)
                    {
                        helperNumberToCheckHowManyLinesIExecutedAboutDepenedencies++;
                        Package p = new Package();

                        string[] arrayString = line.Split(",");

                        p.PackagedName = arrayString[0];
                        p.Version = arrayString[1];
                        packagesToInstall.Packages.Add(p);

                        bool flag = true;


                        Package dep = null;
                        for (int i = 2; i < arrayString.Length; i++)
                        {
                            if (flag)
                            {
                                dep = new Package();
                                dep.PackagedName = arrayString[i];
                                flag = false;
                            }
                            else
                            {
                                dep.Version = arrayString[i];
                                p.Dependencies.Add(dep);
                                dep = null;
                                flag = true;
                            }
                        }



                        if (helperNumberToCheckHowManyLinesIExecutedAboutDepenedencies == packagesToInstall.NumberPackagesToInstall)
                        {
                            listOfPackagesToInstallInThisConfigFile.Add(packagesToInstall);
                            packagesToInstall = null;
                            helperNumberToCheckHowManyLinesIExecutedAboutDepenedencies = 0;
                        }
                    }
                }
                else
                {
                    continue;
                    //throw new Exception("Empty Line in config file");
                }

            }

            return listOfPackagesToInstallInThisConfigFile;
        }

        public static void CreateFile(string fileName, string text)
        {
            try
            {
                // Check if file already exists. If yes, delete it.
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(title, 0, title.Length);
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }


}
