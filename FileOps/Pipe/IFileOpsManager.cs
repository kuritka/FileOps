using System;

namespace FileOps.Pipe
{
    public interface IFileOpsManager
    {
        void Execute();

        IFileOpsManager AddStep(IStep<IAggregate, IAggregate> step);

        IAggregate Context { get;  }

        Action<IAggregate> OnStepProcessed { get; set; }

        Action<IAggregate> OnStart { get; set; }

        Action<IAggregate> OnEnd { get; set; }

        Action<IAggregate, Exception> OnExceptionOccured { get; set; }

    }
}
