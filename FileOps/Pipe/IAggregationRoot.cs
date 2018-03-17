using System.IO;

namespace FileOps.Pipe
{
    public interface IAggregate
    {
        void Add(FileInfo leadFile);

        void Load();

        void AddOrUpdate();

    }
}
