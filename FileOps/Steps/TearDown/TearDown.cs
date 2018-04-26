using FileOps.Pipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileOps.Steps.TearDown
{
    internal class TearDown : IStep<IAggregate, IAggregate>
    {
        public IAggregate Execute(IAggregate toProcess)
        {
            return toProcess;
        }
    }
}
