using FileOps.Configuration.Entities;
using FileOps.Pipe;
using FileOps.Processors.Channels;
using System;

namespace FileOps.Steps.To
{
    public class To : IStep<IAggregate, IAggregate>
    {
        private readonly ToSettings _settings;

        public To(ToSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IAggregate Execute(IAggregate toProcess)
        {
            IChannel channel = ChannelFactory.Create(_settings, toProcess.WorkingDirectory);
            
            channel.Copy();

            return toProcess;

        }
    }
}
