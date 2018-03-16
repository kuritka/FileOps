using System;
using System.Collections.Generic;

namespace FileOps.Pipe
{
    public class FileOpsManager : IFileOpsManager
    {
        private LinkedList<IStep<IEnumerable<IContext>, IEnumerable<IContext>>> _steps;

        public FileOpsManager(LinkedList<IStep<IEnumerable<IContext>, IEnumerable<IContext>>> steps)
        {
            _steps = steps?? throw new ArgumentNullException(nameof(steps));
        }

        public void Execute()
        {
            IEnumerable<IContext> context = new List<IContext>();

            foreach (var step in _steps)
            {
                context = step.Execute(context);
            }
        }
    }
}
