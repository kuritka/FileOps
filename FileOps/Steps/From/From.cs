using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System.Collections.Generic;

namespace FileOps.Steps.From
{
    public class From : IStep<IEnumerable<IContext>, IEnumerable<IContext>>
    {

        private readonly FromSettings _settings;

        public From(FromSettings settings)
        {
            _settings = settings;
        }


        public IEnumerable<IContext> Execute(IEnumerable<IContext> toProcess)
        {
            throw new System.NotImplementedException();
        }
    }
}
