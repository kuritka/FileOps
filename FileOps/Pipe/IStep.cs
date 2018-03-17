using System.Collections.Generic;

namespace FileOps.Pipe
{
    public interface IStep<out TOut, in TIn> 
       where TOut : IAggregate
    {
        TOut Execute(TIn toProcess);        
    }
}
