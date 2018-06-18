using System;
using System.IO;

namespace FileOps.Pipe
{
    public interface IAggregate
    {
        void ExecuteStep(IStep step);

        Guid Guid { get;}

        DirectoryInfo WorkingDirectory { get; }

        IStepContext Current { get; }
    }
}
