using FileOps.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOps.Pipe
{
    public class StepContext : IStepContext
    {
        private readonly List<FileInfo> _files = new List<FileInfo>();

        private readonly IReadOnlyCollection<FileInfo> _previousFiles;

        private readonly Guid _stepGuid = Guid.NewGuid();

        private readonly DateTime _processingDate = DateTime.UtcNow;

        private readonly Guid _guid;

        private readonly IStep _processingStep;

        private readonly DirectoryInfo _workingDirectory;

        public StepContext(IStepContext previousStepContext) : this(previousStepContext.ProcessingStep, previousStepContext.Guid, previousStepContext.WorkingDirectory)
        {
            if (previousStepContext == null) throw new ArgumentNullException($"{previousStepContext}");
            _previousFiles =  previousStepContext.Files.ToList();
        }


        public StepContext(IStep processingStep, Guid guid, DirectoryInfo workingDirectory)
        {
            _workingDirectory = workingDirectory;

            _guid = guid;

            _processingStep = processingStep ?? throw new ArgumentNullException($"{processingStep}");
        }

        public void Attach(FileInfo file)
        {
            file.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(Constants.OneGB);

            _files.Add(file);
        }

        public void Attach(IEnumerable<FileInfo> files)
        {
            files.ThrowExceptionIfNull();

            foreach (var file in files)
            {
                Attach(file);
            }
        }

        public IEnumerable<FileInfo> Files => _files;

        public IStep ProcessingStep => _processingStep;

        public DirectoryInfo WorkingDirectory => _workingDirectory;

        public Guid Guid => _guid;

        public IEnumerable<FileInfo> PreviousFiles => _previousFiles;
    }
}
