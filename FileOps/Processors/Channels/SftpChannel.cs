using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileOps.Configuration.Entities;

namespace FileOps.Processors.Channels
{
    internal class SftpChannel : IChannel
    {
        private DirectoryInfo processingDirectory;
        private FromSettings channelSettings;
        private ChannelDirectionEnum channelDirection;
        private ToSettings channelSettings1;

        public SftpChannel(DirectoryInfo processingDirectory, FromSettings channelSettings, ChannelDirectionEnum channelDirection)
        {
            this.processingDirectory = processingDirectory;
            this.channelSettings = channelSettings;
            this.channelDirection = channelDirection;
        }

        public SftpChannel(DirectoryInfo processingDirectory, ToSettings channelSettings1, ChannelDirectionEnum channelDirection)
        {
            this.processingDirectory = processingDirectory;
            this.channelSettings1 = channelSettings1;
            this.channelDirection = channelDirection;
        }

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
