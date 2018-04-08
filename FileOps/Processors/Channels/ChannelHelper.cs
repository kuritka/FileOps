using FileOps.Common;
using FileOps.Configuration.Entities;
using Renci.SshNet;
using System.IO;

namespace FileOps.Processors.Channels
{
    internal static class ChannelHelper
    {
        //separation of concern issue + it validates as well :(
        public static (DirectoryInfo, DirectoryInfo) GetSourceOrTarget(ChannelSettings settings, DirectoryInfo workingDirectory)
        {
            var channelDirection = ChannelDirectionFactory.Get(settings);

            if (channelDirection == ChannelDirection.Inbound)
            {
                return (settings.Path.AsDirectoryInfo().ThrowExceptionIfNullOrDoesntExists(),
                    workingDirectory);
            }
            else
            {
                return (workingDirectory.ThrowExceptionIfNullOrDoesntExists(),
                 settings.Path.AsDirectoryInfo().ThrowExceptionIfNullOrDoesntExists());
            }
        }



        public static ConnectionInfo AsConnectionInfo(this ChannelSettings channelSettings)
        {
            FileInfo privateKey = new FileInfo(channelSettings.PrivateKey);
            if (!File.Exists(privateKey.FullName)) throw new FileNotFoundException($"File with private key doesn't exist. {privateKey.FullName}");
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(privateKey.FullName);
            PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod = new PrivateKeyAuthenticationMethod(channelSettings.UserName, privateKeyFile);
            int port = channelSettings.Port <= 0 ? Constants.DefaultSftpPort : channelSettings.Port;
            return new ConnectionInfo(channelSettings.Host, port, channelSettings.UserName, privateKeyAuthenticationMethod);
        }

    }
}
