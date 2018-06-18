using FileOps.Configuration.Entities;
using System.IO;

namespace FileOps.Tests.Common.Data
{
    public static class FileProvider
    {
        public static class Configuration
        {
            public static readonly FileInfo SettingsInXml = new FileInfo(Path.Combine("Common", "Data", "Configuration", "XMLFile1.xml"));

            public static readonly FileInfo NotExistingJSonFile = new FileInfo(Path.Combine("Common", "Data", "Configuration", "NotExists.json"));

            public static readonly FileInfo SettingsZipping = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1.json"));

            public static readonly FileInfo SettingsZippingWithPassword= new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1WithZipPassword.jSON"));

            public static readonly FileInfo SettingsZippingWithUpdatedPassword = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1WithZipPassword2.json"));

            public static readonly FileInfo SettingsUnzipping = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings2.json"));

            public static readonly FileInfo SettingsZippingWithUnsupportedValues = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1WithUnsupportedProperties.json"));

            public static readonly FileInfo SettingsZippingCorrupted = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1Corrupted.json"));

            public static readonly FileInfo Settings2 = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings2.JSON"));

            public static readonly FileInfo SharedSettings = new FileInfo(Path.Combine("Common", "Data", "Configuration", "shared.settings.json"));

            public static readonly FileInfo SharedSettings2 = new FileInfo(Path.Combine("Common", "Data", "Configuration", "shared.settings2.json"));

            public static readonly FileInfo SettingsZippingWithReferences = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1WithReferences.json"));

            public static readonly FileInfo SettingsZippingWithReferencesShared = new FileInfo(Path.Combine("Common", "Data", "Configuration", "settings1WithReferences.Shared.json"));
           
        }


        public static class Sftp
        {

            public static readonly FromSettings FromSettings = new FromSettings()
            {
                Path = "/home/ec2-user/From",
                Type = ConfigChannelType.Sftp,
                PrivateKey = new DirectoryInfo(Path.Combine("Common", "key.private")).FullName,
                Host = "127.0.0.1",
                Port = 22,
                UserName = "ec2-user",
            };


            public static readonly FromSettings EmptySettings = new FromSettings()
            {
                Path = "/home/ec2-user/Empty",
                Type = ConfigChannelType.Sftp,
                PrivateKey = new DirectoryInfo(Path.Combine("Common", "key.private")).FullName,
                Host = "127.0.0.1",
                Port = 22,
                UserName = "ec2-user",
            };



            public static readonly FromSettings ToSettings = new FromSettings()
            {
                Path = "/home/ec2-user/To",
                Type = ConfigChannelType.Sftp,
                PrivateKey = new DirectoryInfo(Path.Combine("Common", "key.private")).FullName,
                Host = "127.0.0.1",
                Port = 22,
                UserName = "ec2-user",
            };
        }

    }
}
