using System;
using System.Collections.Generic;
using System.IO;
using FileOps.Configuration.Entities;
using FileOps.Common;

namespace FileOps.Processors.Channels
{
    internal class LocalChannel : IChannel
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly ChannelSettings _channelSettings;
        private readonly DirectoryInfo _workingDirectory;




        public LocalChannel(DirectoryInfo workingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            ChannelDirectionEnum channelDirection = ChannelDirectionFactory.Get(channelSettings);

            if (channelDirection == ChannelDirectionEnum.Inbound)
            {
                _target = workingDirectory;

                _source = _channelSettings.Path.AsDirectoryInfo().ThrowExceptionIfNullOrDoesntExists();
            }
            else
            {
                _source = workingDirectory.ThrowExceptionIfNullOrDoesntExists();

                _target = _channelSettings.Path.AsDirectoryInfo().ThrowExceptionIfNullOrDoesntExists();
            }

        }


        public IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles)
        {
            //Download source directory content to target directory.
            IList<FileInfo> fileInfoList = new List<FileInfo>();

            try
            {
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    string targetPath = Path.Combine(_target.FullName, Path.GetFileName(sourceFile.Name));

                    // Copy with overwriting.
                    File.Create($"{sourceFile.FullName}{Constants.FileExtensions.FileOps}").Close();

                    File.Copy(sourceFile.FullName, targetPath, true);

                    fileInfoList.Add(new FileInfo(targetPath));
                }
                return fileInfoList;
            }
            catch (Exception)
            {
                Directory.Delete(_target.FullName);
                throw;
            }

        }

        public void CreateSuffixFiles(IEnumerable<FileInfo> sourceFiles)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> Delete(IEnumerable<FileInfo> sourceFiles)
        {
            throw new NotImplementedException();
        }

        public int GetCount(FileInfo sourceFile)
        {
            throw new NotImplementedException();
        }
     
    }
}
