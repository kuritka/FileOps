namespace FileOps.Configuration.Entities
{
    internal interface ISettings
    {
        string Identifier { get; set; }

        string GroupIdentifier { get; set; }
    }
}
