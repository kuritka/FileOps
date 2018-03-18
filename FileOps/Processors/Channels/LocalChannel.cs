using System;
using System.Collections.Generic;
using System.IO;
using FileOps.Configuration.Entities;

namespace FileOps.Processors.Channels
{
    internal class LocalChannel : IChannel
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly ChannelSettings _channelSettings;
        private readonly ChannelDirectionEnum _channelDirection;
        private readonly string _fileMask;
        private readonly string _successFileUploadSuffix;
        private readonly IList<string> _excludeFileNameWithParts;
        private DirectoryInfo sourceDirectoryInfo;
        private DirectoryInfo targetDirectoryInfo;
        private FromSettings channelSettings;
        private ToSettings channelSettings1;

        public LocalChannel(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo, FromSettings channelSettings)
        {
            this.sourceDirectoryInfo = sourceDirectoryInfo;
            this.targetDirectoryInfo = targetDirectoryInfo;
            this.channelSettings = channelSettings;
        }

        public LocalChannel(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo, ToSettings channelSettings1)
        {
            this.sourceDirectoryInfo = sourceDirectoryInfo;
            this.targetDirectoryInfo = targetDirectoryInfo;
            this.channelSettings1 = channelSettings1;
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
