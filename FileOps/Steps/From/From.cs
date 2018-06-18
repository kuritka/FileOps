using FileOps.Configuration.Entities;
using FileOps.Pipe;
using FileOps.Processors.Channels;
using System;

namespace FileOps.Steps.From
{
    public class From : IStep
    {

        private readonly FromSettings _settings;

        public From(FromSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }


        public void Execute(IStepContext context)
        {
            context.WorkingDirectory.Create();

            IChannel channel = ChannelFactory.Create(_settings, context.WorkingDirectory);

            var processedFiles = channel.Copy();

            context.Attach(processedFiles);
        }
    }
}
