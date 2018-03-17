using FileOps.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;


namespace FileOps.Configuration
{
    internal class ConfigurationFactory : IConfigurationFactory
    {
        public T Get<T>(IEnumerable<FileInfo> configFiles)
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

            T result = merged.ToObject<T>();

            return result;
        }
      
    }
}
