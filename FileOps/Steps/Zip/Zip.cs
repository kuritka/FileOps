using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;
using System.Collections.Generic;

namespace FileOps.Steps.Zip
{
    public class Zip : IStep<IEnumerable<IContext>, IEnumerable<IContext>>
    {
        ZipSettings _settings;

        public Zip(ZipSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<IContext> ExecuteStep(IEnumerable<IContext> toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
