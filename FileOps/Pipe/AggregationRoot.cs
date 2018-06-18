using FileOps.Steps.Init;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    internal class AggregationRoot : IAggregate
    {

        private readonly Guid _guid;
        private readonly string _identifier;
        private DirectoryInfo _workingDirectory;

        private readonly Stack<IStepContext> _contexts = new Stack<IStepContext>();

        private readonly IStepContext _emptyStepContext;


        public AggregationRoot(string identifier)
        {
            _guid = Guid.NewGuid();

            _identifier = string.IsNullOrEmpty(identifier) ? throw new ArgumentNullException($"{identifier}") : identifier;

            AttachWorkingDirectory();

            _emptyStepContext = new StepContext(new Init(), _guid, _workingDirectory);

            _contexts.Push(_emptyStepContext);
        }

        public Guid Guid { get => _guid; }

        public DirectoryInfo WorkingDirectory => _workingDirectory;

        public IStepContext Current => _contexts.Peek();
        
        public void ExecuteStep(IStep step)
        {
            if (step == null) throw new ArgumentNullException($"{step}");

            IStepContext currentContext = new StepContext(Current);

            _contexts.Push(currentContext);

            step.Execute(currentContext);
        }



        private void AttachWorkingDirectory()
        {
            DirectoryInfo workingDirectory = new DirectoryInfo($"{_identifier}_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{_guid:N}");

            _workingDirectory = workingDirectory;
        }
    }
}
