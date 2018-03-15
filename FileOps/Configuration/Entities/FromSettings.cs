namespace FileOps.Configuration.Entities
{
    public class FromSettings : ChannelSettings
    {
        public int MaxFileCountToProcess { get; set; }

        public string FileMask { get; set; }

        public string[] ExclusionFileMasks { get; set; }

        public bool IgnoreEmptyFiles { get; set; }
    }
}
