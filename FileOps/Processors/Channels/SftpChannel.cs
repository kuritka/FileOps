using System;
using System.Collections.Generic;
using System.IO;
using FileOps.Configuration.Entities;
using Renci.SshNet;
using FileOps.Common;

namespace FileOps.Processors.Channels
{
    internal class SftpChannel : IChannel
    {
        private const string ConnectionErrorMessage = "Connection to SFTP not established.";
        private const string TmpExtension = ".tmp";
        private readonly ChannelSettings _channelSettings;
        private readonly ChannelDirectionEnum _channelDirection;
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly ConnectionInfo _connectionInfo;


        public SftpChannel(DirectoryInfo workingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            _source = ChannelHelper.GetSourceOrTarget(_channelSettings, workingDirectory).Item1;

            _target = ChannelHelper.GetSourceOrTarget(_channelSettings, workingDirectory).Item2;


            
        }



        public IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles)
        {
            using (SftpClient client = new SftpClient(_connectionInfo))
            {
            }
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<FileInfo> targetFiles)
        {
            throw new NotImplementedException();
        }
    }
}
