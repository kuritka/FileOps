using FileOps.Configuration.Entities;
using System;
using System.Collections.Generic;

namespace FileOps.Pipe
{
    public class FileOpsManager : IFileOpsManager
    {
        private readonly LinkedList<IStep<IAggregate, IAggregate>> _steps;

        private IAggregate _aggregate = new AggregationRoot();

        public FileOpsManager(LinkedList<IStep<IAggregate, IAggregate>> steps)
        {
            _steps = steps?? throw new ArgumentNullException(nameof(steps));
        }

        public Action<IAggregate> Status;

        public void Execute()
        {
            
            foreach (var step in _steps)
            {
                Status.Invoke(_aggregate);

                _aggregate = step.Execute(_aggregate);
            }
        }

        public IFileOpsManager AddStep(IStep<IAggregate, IAggregate> step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            _steps.AddLast(step);
            return this;
        }

        public IAggregate Context => _aggregate;
    }
}
