using System.Collections.Generic;
using System.IO;

namespace FileOps.Processors.Channels
{
    internal interface IChannel
    {
        IEnumerable<FileInfo> Copy(IEnumerable<FileInfo> sourceFiles);

        IEnumerable<FileInfo> Delete(IEnumerable<FileInfo> sourceFiles);

        void CreateSuffixFiles(IEnumerable<FileInfo> sourceFiles);

        int GetCount(FileInfo sourceFile);
    }
}