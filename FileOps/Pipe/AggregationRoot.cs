using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    internal class AggregationRoot : IAggregate
    {

        private readonly Guid _guid;

        public AggregationRoot()
        {
            _guid = Guid.NewGuid();
        }

        private readonly IList<IContext> _contexts = new List<IContext>();

        public Guid Guid { get => _guid; }

        public void Add(FileInfo leadFile)
        {
            leadFile.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(Constants.OneGB);

            _contexts.Add(new Context(leadFile));
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
