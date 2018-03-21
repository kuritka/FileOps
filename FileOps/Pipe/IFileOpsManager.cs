namespace FileOps.Pipe
{
    public interface IFileOpsManager
    {
        void Execute();

        IFileOpsManager AddStep(IStep<IAggregate, IAggregate> step);

        IAggregate Context { get;  }
        
    }
}
