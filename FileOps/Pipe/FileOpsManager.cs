using FileOps.Common;
using FileOps.Steps.TearDown;
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
            _steps.AddLast(new TearDown());
        }

        public Action<IAggregate> OnStepProcessed;

        public Action<IAggregate> OnStart;

        public Action<IAggregate> OnEnd;

        public Action<IAggregate, Exception> OnExceptionOccured;

        public void Execute()
        {

            OnStart.Invoke(_aggregate);

            try
            {
                foreach (var step in _steps)
                {
                    _aggregate = step.Execute(_aggregate);

                    OnStepProcessed.Invoke(_aggregate);
                }

                _aggregate.WorkingDirectory.DeleteWithContentIfExists();
            }
            catch (Exception ex)
            {
                OnExceptionOccured.Invoke(_aggregate, ex);
            }

            OnEnd.Invoke(_aggregate);
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
