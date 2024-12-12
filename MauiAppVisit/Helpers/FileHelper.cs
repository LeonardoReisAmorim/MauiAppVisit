using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppVisit.Helpers
{
    public static class FileHelper
    {
        private static readonly string _fileName = "filevrinstalled.json";
        public static void CreateFileJSONInDevice(string path)
        {
            string filePath = Path.Combine(path, _fileName);
            if(!File.Exists(filePath)) File.WriteAllText(filePath, "");
        }
        
        public static void WriteJsonVRVersion(string path, string content)
        {
            string filePath = Path.Combine(path, _fileName);
            if (File.Exists(filePath)) File.WriteAllText(filePath, content);
        }

        public static string ReadJsonVRVersion(string path)
        {
            string filePath = Path.Combine(path, _fileName);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }
    }
}
