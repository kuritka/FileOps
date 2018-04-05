using System;
using System.Collections.Generic;
using System.IO;
using FileOps.Configuration.Entities;
using FileOps.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileOps.Processors.Channels
{
    internal class LocalChannel : IChannel
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly ChannelSettings _channelSettings;
        private readonly ChannelDirectionEnum _channelDirection; 


        public LocalChannel(DirectoryInfo workingDirectory, ChannelSettings channelSettings)
        {
            _channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));

            _channelDirection = ChannelDirectionFactory.Get(channelSettings);

            _source = ChannelHelper.GetSourceOrTarget(_channelSettings, workingDirectory).Item1;

            _target = ChannelHelper.GetSourceOrTarget(_channelSettings, workingDirectory).Item2;
        }


        public IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles)
        {
            IList<FileInfo> fileInfoList = new List<FileInfo>();
            try
            {
                var sourceFilesSubset =
                    
                    _channelDirection == ChannelDirectionEnum.Inbound ?
                  sourceFiles
                    .Where(d => 
                            d.IsMatch(((FromSettings)_channelSettings).FileMask, ((FromSettings)_channelSettings).IgnoreUpperCase ?  RegexOptions.IgnoreCase : RegexOptions.None ) && 
                           (((FromSettings)_channelSettings).ExclusionFileMasks.IsNullOrEmpty() ? true  : !d.IsMatch(((FromSettings)_channelSettings).ExclusionFileMasks, 
                           ((FromSettings)_channelSettings).IgnoreUpperCase ? RegexOptions.IgnoreCase : RegexOptions.None)))
                    .Take(Constants.MaxFileCountToProcess)
                    .OrderByDescending(d => d.CreationTime)

                : sourceFiles
                    .Take(Constants.MaxFileCountToProcess)
                    .OrderByDescending(d => d.CreationTime);

                foreach (FileInfo sourceFile in sourceFilesSubset)
                {
                    string targetFilePath = Path.Combine(_target.FullName, Path.GetFileName(sourceFile.Name));

                    // Copy with overwriting.
                    if (_channelDirection == ChannelDirectionEnum.Inbound)
                    {
                        File.Create($"{sourceFile.FullName}{Constants.FileExtensions.FileOps}").Close();
                    }

                    File.Copy(sourceFile.FullName, targetFilePath, true);

                    if(_channelDirection == ChannelDirectionEnum.Outbound)
                    {
                        string suffix = ((ToSettings)_channelSettings).SuccessFileUploadSuffix;
                        if (!string.IsNullOrEmpty(suffix))
                        {
                            File.Create($"{targetFilePath}{suffix}").Close();
                        }
                    }

                    fileInfoList.Add(new FileInfo(targetFilePath));
                }
                return fileInfoList;
            }
            catch (Exception)
            {
                Directory.Delete(_workingDirectory.FullName);
                throw;
            }

        }

      


        public void Delete(IEnumerable<FileInfo> filesToDelete)
        {
            try
            {
                foreach (FileInfo targetFile in filesToDelete)
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
