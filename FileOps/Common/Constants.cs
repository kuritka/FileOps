namespace FileOps.Common
{
    internal static class Constants
    {
        public const int OneMB = 1024 * 1024;
        public const int OneGB = OneMB * 1024;

        public const int MaxFileCountToProcess = 20;

        public const int DefaultSftpPort = 22;

        public static class FileExtensions
        {
            public const string Xml = ".xml";
            public const string Csv = ".csv";
            public const string Zip = ".zip";
            public const string GZip = ".gz";
            public const string P7S = ".p7s";
            public const string Json = ".json";
        }

    }
}
