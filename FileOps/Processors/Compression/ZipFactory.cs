using System;
using FileOps.Configuration.Entities;
using Beamer.Infrastructure.Compression;

namespace FileOps.Processors.Compression
{

    internal interface IZipFactory
    {
        ICompressor Get();
    }

    internal class ZipFactory : IZipFactory
    {
        private readonly ZipSettings _settings;

        public ZipFactory(ZipSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public ICompressor Get()
        {
            return string.IsNullOrEmpty(_settings.Password)
                ? (ICompressor) new Zip(_settings.CompressionLevel)
                : new PasswordProtectedZip(_settings.Password, _settings.CompressionLevel);
        }
    }
}
