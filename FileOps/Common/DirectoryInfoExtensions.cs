using System;
using System.IO;
using System.Linq;

namespace FileOps.Common
{
    internal static class DirectoryInfoExtensions
    {
        public static DirectoryInfo ThrowExceptionIfNullOrDoesntExists(this DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));

            if (!Directory.Exists(directory.FullName)) throw new FileNotFoundException($"Directory {directory.Name} doesn't exists");

            return directory;
        }



        public static DirectoryInfo AsDirectoryInfo(this string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return new DirectoryInfo(path);
            }
            throw new ArgumentNullException(nameof(path));
        }


        public static bool IsEmpty(this DirectoryInfo directory)
        {
            return !Directory.EnumerateFileSystemEntries(directory.FullName).Any();
        }

        public static DirectoryInfo CreateIfNotExists(this DirectoryInfo target)
        {
            if (!Directory.Exists(target.FullName))
            {
                target.Create();
            }
            return target;
        }

        public static void CopyContentTo(this DirectoryInfo directory, DirectoryInfo target)
        {
            if (!Directory.Exists(directory.FullName)) throw new DirectoryNotFoundException(nameof(directory));
            if (!Directory.Exists(target.FullName))
            {
                target.Create();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
                CopyContentTo(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in directory.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }


        public static void DeleteContent(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())  File.Delete( file.FullName);
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static DirectoryInfo DeleteWithContentIfExists(this DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName))
            {
                target.DeleteContent();
                Directory.Delete(target.FullName);
            }
            return target;
        }


    }
}
