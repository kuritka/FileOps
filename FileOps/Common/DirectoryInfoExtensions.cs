using System;
using System.IO;

namespace FileOps.Common
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo ThrowExceptionIfNullOrDoesntExists(this DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));

            if (!Directory.Exists(directory.FullName)) throw new FileNotFoundException($"Directory {directory.Name} doesn't exists");

            return directory;
        }



        public static DirectoryInfo AsDirectoryInfo(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new DirectoryInfo(path);
            }
            throw new ArgumentNullException(nameof(path));
        }


    }
}
