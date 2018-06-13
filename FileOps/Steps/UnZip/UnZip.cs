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
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IAggregate Execute(IAggregate aggregate)
        {
            var compressor = new Processors.Compression.ZipFactory(_settings).Get();

            foreach (var file in aggregate.Current.Files)
            {
                var uncompressedFiles =  compressor.Decompress(file, aggregate.WorkingDirectory);

                aggregate.Add(uncompressedFiles);
            }

            return aggregate;
        }
    }
}
