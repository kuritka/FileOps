using FileOps.Configuration.Entities;
using FileOps.Processors.Channels;
using Renci.SshNet;
using System.IO;
using System.Linq;

namespace FileOps.Common
{
    internal static class SftpExtensions
    {

        public static ChannelSettings DeleteWithContentIfExists(this ChannelSettings target)
        {
            var info = target.AsConnectionInfo();
            using (SftpClient client = new SftpClient(info))
            {
                client.Connect();

                if (client.Exists(target.Path))
                {
                    DeleteRecoursively(target.Path, client);
                }
            }
            return target;
        }

        private static void DeleteRecoursively(string path, SftpClient client)
        {
            var files = client.ListDirectory(path).Where(d=>d.Name != "." && d.Name != "..");
            foreach (var fileSystemInfo in files)
            {
                if (fileSystemInfo.IsDirectory)
                {
                    client.Open(fileSystemInfo.FullName, FileMode.Open);
                    DeleteRecoursively(fileSystemInfo.FullName, client);
                }
                else
                {
                    client.Delete(fileSystemInfo.FullName);
                }
            }
            client.Delete(path);
        }

        public static ChannelSettings CreateIfNotExists(this ChannelSettings target)
        {
            var info = target.AsConnectionInfo();
            using (SftpClient client = new SftpClient(info))
            {
                client.Connect();
                if (!client.Exists(target.Path))
                {
                    client.CreateDirectory(target.Path);
                }
            }
            return target;
        }


        public static void CopyContentTo(this DirectoryInfo directory, ChannelSettings target)
        {
            var info = target.AsConnectionInfo();
            var files = directory.GetFiles();
            using (SftpClient client = new SftpClient(info))
            {
                client.Connect();
                foreach (var file in files)
                {
                    string tempFile = $"{Path.GetFileNameWithoutExtension(file.Name)}{Constants.FileExtensions.FileOps}";
                    string tempFileName = Path.Combine(target.Path, tempFile);
                    string originalFileName = Path.Combine(target.Path, file.Name);
                    using (var stream = file.OpenRead())
                    {
                        client.UploadFile(stream, tempFileName, true);
                    }
                    client.RenameFile(tempFileName, originalFileName);
                }
            }
        }
    }
}
