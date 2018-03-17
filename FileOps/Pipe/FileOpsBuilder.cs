using System;
using System.Collections.Generic;
using FileOps.Pipe;
using System.IO;
using FileOps.Common;
using FileOps.Configuration.Entities;
using FileOps.Configuration;

namespace FileOps
{
    public class FileOpsBuilder : IFileOpsBuilder
    {
        private Dictionary<string, FileInfo> _configFiles;

        public FileOpsBuilder()
        {
            _configFiles = new Dictionary<string, FileInfo>();
        }

        public IFileOpsBuilder AddConfiguration(FileInfo jsonFile)
        {
            jsonFile
                .ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfExtensionIsDifferentFrom(Constants.FileExtensions.Json)
                .ThrowExceptionIfFileSizeExceedsMB(1);
            try
            {
                _configFiles.Add(jsonFile.Name, jsonFile);

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{jsonFile.Name} is already added.", ex);
            }
            return this;
        }


        public LinkedList<IStep<IAggregate, IAggregate>> Build()
        {
            IStepFactory stepFactory = new StepFactory();

            var settings = new ConfigurationFactory().Get<Settings>(_configFiles.Values);

            if (settings.Pipe == null) settings.Pipe = new Settings.Step[0];

            if (settings.Common == null) settings.Common = new Settings.CommonRecord[0];

            var steps = stepFactory.Get(settings);

            return new LinkedList<IStep<IAggregate, IAggregate>>(steps);
        }
    }
}
