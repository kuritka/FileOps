using System.Collections.Generic;

namespace FileOps.Pipe
{
    public interface IStep
    {
        void Execute(IStepContext stepContext);        
    }
}
