using FileOps.Common;

namespace FileOps.Configuration.Entities
{
    public class FromSettings : ChannelSettings
    {

        public FromSettings()
        {
            //Setting default values
            MaxFileCountToProcess = Constants.MaxFileCountToProcess;
        }

        public int MaxFileCountToProcess { get; set; }

        public string FileMask { get; set; }

        public string[] ExclusionFileMasks { get; set; }

        public bool IgnoreEmptyFiles { get; set; }
    }
}
