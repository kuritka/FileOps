﻿using System;
using System.Collections.Generic;

namespace FileOps.Pipe
{
    public class FileOpsManager : IFileOpsManager
    {
        private LinkedList<IStep<IAggregate, IAggregate>> _steps;

        private IAggregate _aggregate = new AggregationRoot();

        public FileOpsManager(LinkedList<IStep<IAggregate, IAggregate>> steps)
        {
            _steps = steps?? throw new ArgumentNullException(nameof(steps));
        }


        public void Execute()
        {
            foreach (var step in _steps)
            {
                _aggregate = step.Execute(_aggregate);
            }
        }

        public IAggregate Context => _aggregate;
    }
}
