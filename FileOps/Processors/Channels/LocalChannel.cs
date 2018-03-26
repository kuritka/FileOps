using System;
using System.Collections.Generic;
using System.IO;
using FileOps.Configuration.Entities;
using FileOps.Common;
using System.Linq;

namespace FileOps.Processors.Channels
{
    internal class LocalChannel : IChannel
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly ChannelSettings _channelSettings;
        private readonly DirectoryInfo _workingDirectory;
        private readonly ChannelDirectionEnum _channelDirection; 



        public LocalChannel(DirectoryInfo workingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            if (_channelDirection == ChannelDirectionEnum.Inbound)
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
            IList<FileInfo> fileInfoList = new List<FileInfo>();
            try
            {
                var sourceFilesSubset = sourceFiles
                    .Take(Constants.MaxFileCountToProcess)
                    .OrderByDescending(d => d.CreationTime);

                foreach (FileInfo sourceFile in sourceFilesSubset)
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

        public void CreateSuffixFiles(IEnumerable<FileInfo> targetFiles)
        {
            IList<FileInfo> fileInfoList = new List<FileInfo>();
            if(_channelDirection == ChannelDirectionEnum.Inbound)
            {
                throw new InvalidOperationException($"Trying create suufix file for {ChannelDirectionEnum.Inbound} direction");
            }
            try
            {
                foreach (FileInfo targetFile in targetFiles)
                {
                    // Copy with overwriting.
                    File.Create($"{targetFile.FullName}{((ToSettings)_channelSettings).SuccessFileUploadSuffix}").Close();
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Error at creating suffix files ", ex);
            }
        }


        public void Delete(IEnumerable<FileInfo> targetFiles)
        {
            try
            {
                foreach (FileInfo targetFile in targetFiles)
                {
                    File.Delete(targetFile.FullName);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error at creating suffix files ", ex);
            }
        }
    }
}
