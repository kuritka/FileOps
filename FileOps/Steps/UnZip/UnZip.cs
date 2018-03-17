using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;

namespace FileOps.Steps.UnZip
{
    public class UnZip : IStep<IAggregate, IAggregate>
    {
        ZipSettings _settings;

        public UnZip(ZipSettings settings)
        {
            _settings = settings;
        }

        public IAggregate Execute(IAggregate toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
