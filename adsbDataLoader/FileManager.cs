using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adsbDataLoader
{
    class FileManager
    {
        public static string[] getAdsbFiles(string rootPath)
        {
            return Directory.GetFiles(rootPath, "adsbdata*.txt", SearchOption.AllDirectories);
        }

        public static int getLineCount(string filename)
        {
            return File.ReadLines(filename).Count();
        }

        public static string getParentDir(string filePath)
        {
            string[] pathParts = filePath.Split('\\');
            return pathParts[8]; 
        }
    }
}
