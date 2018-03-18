namespace FileOps.Pipe
{
    public interface IFileOpsManager
    {
        void Execute();

        IAggregate Context { get;  }
        
    }
}
