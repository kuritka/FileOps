namespace FileOps.Configuration.Entities
{
    public class ZipSettings : BaseSettings
    {
        public CompressionLevel CompressionLevel { get; set; }

        public string Password { get; set; }

        //if true then abc.xml -> abc.xml.zip otherwise it makes abc.zip
        public string AddExtension { get; set; }

    }
}
