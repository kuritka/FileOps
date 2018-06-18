using FileOps.Configuration.Entities;
using System.Collections.Generic;

namespace FileOps.Pipe
{
    internal interface IStepFactory
    {
        IEnumerable<IStep> Get(Settings settings);
    }
}
