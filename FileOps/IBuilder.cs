using FileOps.Pipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileOps
{
    public interface IFileOpsBuilder
    {
        LinkedList<IStep<IEnumerable<IContext>, IEnumerable<IContext>>> Build();

        IFileOpsBuilder AddConfiguration(FileInfo jsonFile);
    }
}
