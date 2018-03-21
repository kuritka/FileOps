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
        private readonly ChannelDirectionEnum _channelDirection;

       

        public LocalChannel(DirectoryInfo processingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            processingDirectory.ThrowExceptionIfNullOrDoesntExists();

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            DirectoryInfo settingsDirectory = _channelSettings.Path.AsDirectoryInfo().ThrowExceptionIfNullOrDoesntExists(); 

            if (_channelDirection == ChannelDirectionEnum.Inbound)
            {
                _target = processingDirectory;

                _source = settingsDirectory;
            }
            else
            {
                _source = processingDirectory;

                _target = settingsDirectory;
            }
        }

        public string SourceFullPath => _source.FullName;
        public string TargetFullPath => _target.FullName;


       

        public IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles)
        {
            //Download source directory content to target directory.
            IList<FileInfo> fileInfoList = new List<FileInfo>();

            try
            {
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    string targetPath = Path.Combine(_target.FullName, Path.GetFileName(sourceFile.FullName));

                    // Copy with overwriting.
                    File.Create($"{targetPath}{Constants.FileExtensions.FileOps}");

                    File.Copy(sourceFile.FullName, targetPath, true);

                    fileInfoList.Add(new FileInfo(targetPath));
                }
                return fileInfoList;
            }
            catch (Exception)
            {
                foreach (FileInfo fileInfo in fileInfoList)
                {
                    if (File.Exists(fileInfo.FullName))
                    {
                        fileInfo.Delete();
                    }
                }
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

        public IEnumerable<FileInfo> Rename(IEnumerable<FileInfo> sourceFiles)
        {
            throw new NotImplementedException();
        }


    }
}
