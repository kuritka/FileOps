using System;
using System.Collections.Generic;
using FileOps.Pipe;
using System.IO;
using FileOps.Common;
using FileOps.Configuration;
using FileOps.Configuration.Entities;
using System.Linq;

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

                _configFiles.Add( jsonFile.Name, jsonFile);

            }catch(Exception ex)
            {
                throw new ArgumentException($"{jsonFile.Name} is already added.",ex);
            }
            return this;
        }

        public LinkedList<IStep<IEnumerable<IContext>, IEnumerable<IContext>>> Build()
        {
            Settings settings = new Settings();

            IConfigurationFactory configurationFactory = new ConfigurationFactory();

            IStepFactory stepFactory = new StepFactory();

            foreach (var configFile in _configFiles)
            {
                var tempSettings = configurationFactory.Get<Settings>(configFile.Value);

                MergeSettings(settings, tempSettings);
            }
            
            var steps = stepFactory.Get(settings);

            return new LinkedList<IStep<IEnumerable<IContext>, IEnumerable<IContext>>>(steps);
        }


       


        private void MergeSettings<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }

    }
}
