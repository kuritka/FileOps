using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    public interface IAggregate
    {
        void Add(FileInfo file);

        void Add(IEnumerable<FileInfo> files);

        void Load();

        void AddOrUpdate();

        Guid Guid { get;}

        void AttachWorkingDirectory(DirectoryInfo directory);

        DirectoryInfo WorkingDirectory { get; }

        IStepContext Current { get; }
    }
}
