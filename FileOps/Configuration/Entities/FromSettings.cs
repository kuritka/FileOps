using FileOps.Common;

namespace FileOps.Configuration.Entities
{
    public class FromSettings : ChannelSettings,  ISettings
    {

        public FromSettings()
        {
            //Setting default values
            MaxFileCountToProcess = Constants.MaxFileCountToProcess;
        }

        public int MaxFileCountToProcess { get; set; }

        public string FileMask { get; set; }

        public string[] ExclusionFileMasks { get; set; }

        public string Identifier { get; set; }

        public string GroupIdentifier { get; set; }

        public bool IgnoreCaseSensitive { get; set; }
    }
}
