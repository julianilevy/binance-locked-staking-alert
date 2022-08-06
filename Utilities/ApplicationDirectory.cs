using System.IO;
using System.Reflection;

namespace BinanceLockedStakingAlert
{
    public class ApplicationDirectory
    {
        public static string GetPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public static string GetFilePath(string fileName)
        {
            return GetFilePath(fileName, null);
        }

        public static string GetFilePath(string fileName, params string[] remainingPath)
        {
            var filePath = GetPath();

            if (remainingPath != null)
            {
                foreach (var folder in remainingPath)
                    filePath += Path.DirectorySeparatorChar + folder;
            }

            filePath += Path.DirectorySeparatorChar + fileName;

            return filePath;
        }

        public static void CreateFolder(string folderName)
        {
            CreateFolder(folderName, null);
        }

        public static void CreateFolder(string folderName, params string[] remainingPath)
        {
            var folderPath = GetPath();

            if (remainingPath != null)
            {
                for (int i = 0; i < remainingPath.Length; i++)
                    folderPath += Path.DirectorySeparatorChar + remainingPath[i];
            }

            if (Directory.Exists(folderPath + Path.DirectorySeparatorChar + folderName))
                Directory.Delete(folderPath + Path.DirectorySeparatorChar + folderName, true);

            Directory.CreateDirectory(folderPath + Path.DirectorySeparatorChar + folderName);
        }
    }
}