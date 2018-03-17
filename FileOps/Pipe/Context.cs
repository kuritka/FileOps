using FileOps.Common;
using System;
using System.IO;

namespace FileOps.Pipe
{
    internal class Context : IContext
    {
        private readonly FileInfo _leadingFile;

        private readonly Guid _guid;

        private readonly DateTime _processingDate;

        public Context(FileInfo leadingFile)
        {
            leadingFile.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(Constants.OneGB);

            _guid = Guid.NewGuid();

            _leadingFile = leadingFile;

            _processingDate = DateTime.UtcNow;
        }

        public FileInfo File => _leadingFile;


        
    }
}
