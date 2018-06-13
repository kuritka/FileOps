using FileOps.Configuration.Entities;
using FileOps.Pipe;
using FileOps.Processors.Channels;
using System;
using System.IO;

namespace FileOps.Steps.From
{
    public class From : IStep<IAggregate, IAggregate>
    {

        private readonly FromSettings _settings;

        public From(FromSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }


        public IAggregate Execute(IAggregate aggregate)
        {

            DirectoryInfo workingDirectory = new DirectoryInfo($"{_settings.Identifier}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{aggregate.Guid:N}");

            workingDirectory.Create();

            aggregate.AttachWorkingDirectory(workingDirectory);

            IChannel channel = ChannelFactory.Create(_settings, workingDirectory);

            var processedFiles = channel.Copy();

            aggregate.Add(processedFiles);

            return aggregate;
        }
    }
}
