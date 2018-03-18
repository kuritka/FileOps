using FileOps.Common;
using FileOps.Configuration.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOps.Configuration
{
    internal class ConfigurationFactory : IConfigurationFactory
    {
        public Settings Get(IEnumerable<FileInfo> configFiles)
        {
            if (configFiles == null) throw new ArgumentNullException(nameof(configFiles));

            JObject merged = new JObject();

            foreach (var configFile in configFiles)
            {
                configFile
                    .ThrowExceptionIfNullOrDoesntExists()
                    .ThrowExceptionIfFileSizeExceedsMB(1)
                    .ThrowExceptionIfExtensionIsDifferentFrom(Constants.FileExtensions.Json);

                try
                {
                    JObject json = JObject.Parse(File.ReadAllText(configFile.FullName));

                    merged.Merge(json, new JsonMergeSettings {
                        MergeArrayHandling = MergeArrayHandling.Replace });
                }
                catch (JsonReaderException ex)
                {
                    throw new ArgumentException($"{configFile.Name} is corrupted", ex);
                }
            }

            Settings settings = merged.ToObject<Settings>();

            MapReferencesToSteps(settings);

            return settings;
        }



        /// <summary>
        /// It merge StepSettings if Reference attribute points to shared settings
        /// </summary>
        private void MapReferencesToSteps(Settings settings)
        {
            if (settings.Pipe == null) settings.Pipe = new Settings.Step[0];

            if (settings.Common == null) settings.Common = new Settings.CommonRecord[0];

            foreach (var step in settings.Pipe)
            {
                if (!step.Reference.IsNullOrEmpty())
                {
                    var reference = settings.Common.FirstOrDefault(d => d.Name == step.Reference);
                    if (reference == null)
                    {
                        throw new JsonException($"Parsing configuration error for reference {step.Reference}");
                    }
                    reference.StepSettings.Merge(step.StepSettings, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Merge
                    });
                    step.StepSettings = reference.StepSettings;
                }
            }
        }
      
    }
}
