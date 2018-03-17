using FileOps.Configuration.Entities;
using FileOps.Pipe;

namespace FileOps.Steps.From
{
    public class From : IStep<IAggregate, IAggregate>
    {

        private readonly FromSettings _settings;

        public From(FromSettings settings)
        {
            _settings = settings;
        }


        public IAggregate Execute(IAggregate toProcess)
        {
            throw new System.NotImplementedException();
        }
    }
}
