using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;
using System.Collections.Generic;

namespace FileOps.Steps.Zip
{
    public class Zip : IStep<IAggregate, IAggregate>
    {
        ZipSettings _settings;

        public Zip(ZipSettings settings)
        {
            _settings = settings;
        }

        public IAggregate Execute(IAggregate toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
