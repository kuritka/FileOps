using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;
using System.Collections.Generic;

namespace FileOps.Steps.To
{
    public class To : IStep<IEnumerable<IContext>, IEnumerable<IContext>>
    {
        private readonly ToSettings _settings;

        public To(ToSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<IContext> Execute(IEnumerable<IContext> toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
