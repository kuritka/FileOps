namespace FileOps.Configuration.Entities
{
    public class ToSettings : ChannelSettings, ISettings
    {
        public string SuccessFileUploadSuffix { get; set; }

        public string Identifier { get; set; }

        public string GroupIdentifier { get; set; }
    }
}
