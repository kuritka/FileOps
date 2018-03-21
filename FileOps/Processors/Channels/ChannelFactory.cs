using FileOps.Common;
using FileOps.Configuration.Entities;
using System;
using System.IO;

namespace FileOps.Processors.Channels
{
    internal class ChannelFactory
    {
        public static IChannel Create(ChannelSettings channelSettings, DirectoryInfo processingDirectory)
        {
            if (channelSettings == null) throw new ArgumentNullException(nameof(channelSettings));

            if (processingDirectory == null) throw new ArgumentNullException(nameof(processingDirectory));

            ChannelDirectionEnum channelDirection = channelSettings.GetType() == typeof(FromSettings) ? ChannelDirectionEnum.Inbound : ChannelDirectionEnum.Outbound;

            switch (channelSettings.Type)
            {
                case ConfigChannelType.Local:
                    {
                        return new LocalChannel(processingDirectory, channelSettings);
                    }
                case ConfigChannelType.Sftp:
                    {
                        if (channelDirection == ChannelDirectionEnum.Inbound)
                        {
                            return new SftpChannel(processingDirectory,(FromSettings) channelSettings, channelDirection);
                        }
                        else
                        {
                            return new SftpChannel(processingDirectory, (ToSettings)channelSettings, channelDirection);
                        }
                    }
                default:
                    {
                        throw new InvalidOperationException($"There is no corresponding channel type for '{channelSettings.Type}'.");
                    }
            }
        }
    }
}
