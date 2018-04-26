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

        private DirectoryInfo _workingDirectory;

        public From(FromSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            
        }


        public IAggregate Execute(IAggregate toProcess)
        {

            DirectoryInfo workingDirectory = new DirectoryInfo($"{_settings.Identifier}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{toProcess.Guid:N}");

            workingDirectory.Create();

            toProcess.AttachWorkingDirectory(workingDirectory);

            IChannel channel = ChannelFactory.Create(_settings, workingDirectory);

            //if (_channelDirection == ChannelDirectionEnum.Inbound)
            //{
            //    _target = processingDirectory;

            //    _source = settingsDirectory;
            //}
            //else
            //{
            //    _source = processingDirectory;

            //    _target = settingsDirectory;
            //}

            channel.Copy();
            throw new System.NotImplementedException();
        }
    }
}
