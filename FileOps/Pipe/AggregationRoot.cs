using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    internal class AggregationRoot : IAggregate
    {

        private readonly Guid _guid;

        private DirectoryInfo _workingDirectory;


        public AggregationRoot()
        {
            _guid = Guid.NewGuid();
        }

        private readonly IList<IStepContext> _contexts = new List<IStepContext>();

        public Guid Guid { get => _guid; }

        public DirectoryInfo WorkingDirectory => _workingDirectory;

        public void Add(FileInfo file)
        {
            file.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(Constants.OneGB);

            _contexts.Add(new StepContext(file));
        }

        public void Add(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                Add(file);
            }
        }

        public IStepContext Current => _contexts[_contexts.Count - 1];

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate()
        {
            throw new NotImplementedException();
        }

        public void AttachWorkingDirectory(DirectoryInfo directory)
        {
            if(_workingDirectory == null)
            {
                _workingDirectory = directory;
            }
            else
            {
                throw new InvalidOperationException("Working directory can be attached only once.");
            }
        }
    }
}
