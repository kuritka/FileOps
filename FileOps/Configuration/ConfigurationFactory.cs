using FileOps.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace FileOps.Configuration
{    
    internal class ConfigurationFactory : IConfigurationFactory
    {
        public T Get<T>(FileInfo file)
        {
            file.ThrowExceptionIfNullOrDoesntExists()
                .ThrowExceptionIfFileSizeExceedsMB(1)
                .ThrowExceptionIfExtensionIsDifferentFrom(Constants.FileExtensions.Json);

            JObject json = JObject.Parse(File.ReadAllText(file.FullName));
            return json.ToObject<T>();
        }
    }
}
