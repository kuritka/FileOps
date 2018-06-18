using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;

namespace FileOps.Steps.Zip
{
    public class Zip : IStep
    {
        ZipSettings _settings;

        public Zip(ZipSettings settings)
        {
            _settings = settings;
        }

        public void Execute(IStepContext context)
        {
            throw new NotImplementedException();
        }
    }
}
