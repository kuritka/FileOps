using System;

namespace FileOps.Processors.Compression
{
    internal static class CompressionExtensions
    {
        public static Ionic.Zlib.CompressionLevel AsIonicCompressionLevel(
            this System.IO.Compression.CompressionLevel commonCompressionLevel)
        {
            switch (commonCompressionLevel)
            {
                case System.IO.Compression.CompressionLevel.Optimal:
                    return Ionic.Zlib.CompressionLevel.BestCompression;
                case System.IO.Compression.CompressionLevel.Fastest:
                    return Ionic.Zlib.CompressionLevel.BestSpeed;
                case System.IO.Compression.CompressionLevel.NoCompression:
                    return Ionic.Zlib.CompressionLevel.None;
            }
            throw new NotImplementedException();
        }
    }
}
