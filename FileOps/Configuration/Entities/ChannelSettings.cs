namespace FileOps.Configuration.Entities
{
    public abstract class ChannelSettings 
    {
        public ConfigChannelType Type { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string PrivateKey { get; set; }                
        public string LocalBackupPath { get; set; }       
    }
}
