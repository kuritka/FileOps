﻿using FileOps.Common;
using FileOps.Steps.TearDown;
using System;
using System.Collections.Generic;

namespace FileOps.Pipe
{
    public class FileOpsManager : IFileOpsManager
    {
        private readonly LinkedList<IStep> _steps;

        private IAggregate _aggregate;

        public FileOpsManager(LinkedList<IStep> steps, string identifier)
        {
            _aggregate = new AggregationRoot(identifier);

            _steps = steps?? throw new ArgumentNullException(nameof(steps));

            _steps.AddLast(new TearDown());
        }

        public Action<IAggregate> OnStepProcessed { get; set; }

        public Action<IAggregate> OnStart { get; set; }

        public Action<IAggregate> OnEnd { get; set; }

        public Action<IAggregate, Exception> OnExceptionOccured { get; set; }

        public void Execute()
        {
           OnStart?.Invoke(_aggregate);

            try
            {
                foreach (var step in _steps)
                {
                    _aggregate.ExecuteStep(step);

                    //_aggregate = step.Execute(_aggregate);

                    OnStepProcessed?.Invoke(_aggregate);
                }

                _aggregate.WorkingDirectory.DeleteWithContentIfExists();
            }
            catch (Exception ex)
            {
                OnExceptionOccured?.Invoke(_aggregate, ex);
#if DEBUG
                throw;
#endif
            }

            OnEnd?.Invoke(_aggregate);
        }

        public IFileOpsManager AddStep(IStep step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            _steps.AddLast(step);
            return this;
        }

        public IAggregate Context => _aggregate;

    }
}
