using System.Collections.Generic;
using System.IO;

namespace FileOps.Processors.Channels
{
    internal interface IChannel
    {
        IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles);

        void Delete(IEnumerable<FileInfo> targetFiles);
    }
}