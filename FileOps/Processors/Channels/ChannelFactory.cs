using FileOps.Configuration.Entities;
using System;
using System.IO;

namespace FileOps.Processors.Channels
{
    internal class ChannelFactory
    {
        public static IChannel Create(ChannelSettings channelSettings, DirectoryInfo workingDirectory)
        {
            if (channelSettings == null) throw new ArgumentNullException(nameof(channelSettings));

            if (workingDirectory == null) throw new ArgumentNullException(nameof(workingDirectory));

            ChannelDirectionEnum channelDirection = channelSettings.GetType() == typeof(FromSettings) ? ChannelDirectionEnum.Inbound : ChannelDirectionEnum.Outbound;

            switch (channelSettings.Type)
            {
                case ConfigChannelType.Local:
                {
                    return new LocalChannel(workingDirectory, channelSettings);
                }
                case ConfigChannelType.Sftp:
                {
                    return new SftpChannel(workingDirectory, channelSettings);
                }
                default:
                    {
                        throw new InvalidOperationException($"There is no corresponding channel type for '{channelSettings.Type}'.");
                    }
            }
        }
    }
}
