using System.Collections.Generic;

namespace FileOps.Pipe
{
    public interface IStep<out TOut, in TIn> 
       where TOut : IEnumerable<IContext>
    {
        TOut ExecuteStep(TIn toProcess);        
    }
}
