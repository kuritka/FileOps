using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    public class StepContext : IStepContext
    {
        private readonly List<FileInfo> _files = new List<FileInfo>();

        private readonly Guid _guid = Guid.NewGuid();

        private readonly DateTime _processingDate = DateTime.UtcNow;


        public StepContext(IEnumerable<FileInfo> files)
        {
            files.ThrowExceptionIfNull();

            foreach (var file in files)
            {
                AttachFile(file);
            };
        }

        public StepContext(FileInfo file)
        {
            AttachFile(file);
        }

        private void AttachFile(FileInfo file)
        {
            file.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(Constants.OneGB);

            _files.Add(file);
        }


        public IEnumerable<FileInfo> Files => _files;
        
    }
}
