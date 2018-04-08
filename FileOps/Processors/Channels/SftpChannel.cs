using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileOps.Common;
using FileOps.Configuration.Entities;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace FileOps.Processors.Channels
{
    internal class SftpChannel : IChannel
    {
        private const string ConnectionErrorMessage = "Connection to SFTP not established.";
        private const string TmpExtension = ".tmp";
        private readonly ChannelDirection _channelDirection;
        private readonly string _source;
        private readonly DirectoryInfo _target;
        private readonly ConnectionInfo _connectionInfo;
        private readonly DirectoryInfo _workingDirectory;
        private readonly ChannelSettings _channelSettings;


        public SftpChannel(DirectoryInfo workingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            _workingDirectory = workingDirectory ?? throw new ArgumentNullException(nameof(channelSettings));

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            _source = string.IsNullOrEmpty(channelSettings.Path) ? throw new ArgumentException(nameof(channelSettings.Path))  :  channelSettings.Path;

            _target = _workingDirectory;

            _connectionInfo = channelSettings.AsConnectionInfo();
        }


        public IEnumerable<FileInfo> Copy()
        {

            IList<FileInfo> fileInfoList = new List<FileInfo>();

            using (SftpClient client = new SftpClient(_connectionInfo))
            {
                try
                {
                    client.OperationTimeout = new TimeSpan(0, 0, Constants.Timeouts.SshOperationTimeoutInMinutes, 0);

                    client.Connect();

                    if (!client.IsConnected) throw new Exception(ConnectionErrorMessage);

                    client.ChangeDirectory(_source);

                    if (_channelDirection == ChannelDirection.Inbound)
                    {
                        IEnumerable<SftpFile> sftpFiles = client.ListDirectory(_channelSettings.Path)
                            .Take(Constants.MaxFileCountToProcess);
                        return CopyFrom(sftpFiles, client);
                    }
                    else
                    {
                        IEnumerable<FileInfo> files = _workingDirectory.GetFiles();
                        return CopyTo(files, client);
                    }
                }
                catch (Exception)
                {
                    Directory.Delete(_workingDirectory.FullName);
                    throw;
                }
            }
        }

        private IEnumerable<FileInfo> CopyTo(IEnumerable<FileInfo> files, SftpClient client)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<FileInfo> CopyFrom(IEnumerable<SftpFile> sftpFiles, SftpClient client)
        {
            IList<FileInfo> downloadedFiles = new List<FileInfo>();
            IList<FileInfo> filesToSort = new List<FileInfo>();
            foreach (SftpFile sftpFile in sftpFiles)
            {
                if (sftpFile.IsDirectory || !IsFileMatch(sftpFile.Name,  ((FromSettings)_channelSettings).FileMask)) continue;
                string resultFileName = Path.Combine(_target.FullName, sftpFile.Name);
                filesToSort.Add(new FileInfo(resultFileName));
            }

            foreach (var fileToDownload in filesToSort)
            {
                var downloaded = Path.Combine(_target.FullName, fileToDownload.Name);
                using (FileStream stream = new FileStream(fileToDownload.Name, FileMode.Create))
                {
                    client.DownloadFile(downloaded, stream);
                }
                downloadedFiles.Add(new FileInfo(downloaded));
            }
            return downloadedFiles;
        }
        //private IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles, SftpClient client)
        //{
        //    IList<FileInfo> fileInfoList = new List<FileInfo>();
        //    try
        //    {
        //        var sourceFilesSubset =

        //            _channelDirection == ChannelDirection.Inbound ?
        //          sourceFiles
        //            .Where(d =>
        //                    d.IsMatch(((FromSettings)_channelSettings).FileMask, ((FromSettings)_channelSettings).IgnoreUpperCase ? RegexOptions.IgnoreCase : RegexOptions.None) &&
        //                   (((FromSettings)_channelSettings).ExclusionFileMasks.IsNullOrEmpty() ? true : !d.IsMatch(((FromSettings)_channelSettings).ExclusionFileMasks,
        //                   ((FromSettings)_channelSettings).IgnoreUpperCase ? RegexOptions.IgnoreCase : RegexOptions.None)))
        //            .Take(Constants.MaxFileCountToProcess)
        //            .OrderByDescending(d => d.CreationTime)

        //        : sourceFiles
        //            .Take(Constants.MaxFileCountToProcess)
        //            .OrderByDescending(d => d.CreationTime);

        //        foreach (FileInfo sourceFile in sourceFilesSubset)
        //        {
        //            string targetFilePath = Path.Combine(_target.FullName, Path.GetFileName(sourceFile.Name));

        //            // Copy with overwriting.
        //            if (_channelDirection == ChannelDirection.Inbound)
        //            {
        //                File.Create($"{sourceFile.FullName}{Constants.FileExtensions.FileOps}").Close();
        //            }

        //            File.Copy(sourceFile.FullName, targetFilePath, true);

        //            if (_channelDirection == ChannelDirection.Outbound)
        //            {
        //                string suffix = ((ToSettings)_channelSettings).SuccessFileUploadSuffix;
        //                if (!string.IsNullOrEmpty(suffix))
        //                {
        //                    File.Create($"{targetFilePath}{suffix}").Close();
        //                }
        //            }

        //            fileInfoList.Add(new FileInfo(targetFilePath));
        //        }
        //        return fileInfoList;
        //    }
        //    catch (Exception)
        //    {
        //        Directory.Delete(_workingDirectory.FullName);
        //        throw;
        //    }
        //}


        public void Delete(IEnumerable<FileInfo> targetFiles)
        {
            throw new NotImplementedException();
        }

        private bool IsFileMatch(string fileName, string fileMask)
        {
            return string.IsNullOrEmpty(fileMask) || new FileInfo(fileName).IsMatch(((FromSettings)_channelSettings).FileMask);
        }
    }
}
