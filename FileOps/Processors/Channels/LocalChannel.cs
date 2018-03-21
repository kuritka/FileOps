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

       

        public LocalChannel(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            _source = sourceDirectoryInfo.ThrowExceptionIfNullOrDoesntExists();

            _target = targetDirectoryInfo.ThrowExceptionIfNullOrDoesntExists();
        }

        public string SourceFullPath => _source.FullName;
        public string TargetFullPath => _target.FullName;


       

        public IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles)
        {
            throw new NotImplementedException();
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
