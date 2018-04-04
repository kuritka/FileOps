namespace FileOps.Configuration.Entities
{
    public class ZipSettings :  ISettings
    {
        public System.IO.Compression.CompressionLevel CompressionLevel { get; set; }

        public string Password { get; set; }

        //if true then abc.xml -> abc.xml.zip otherwise it makes abc.zip
        public string AddExtension { get; set; }

        public string Identifier { get; set; }

        public string GroupIdentifier { get; set; }
    }
}
