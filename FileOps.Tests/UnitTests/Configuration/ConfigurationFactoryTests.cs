using FileOps.Configuration;
using FileOps.Configuration.Entities;
using FileOps.Tests.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Tests.UnitTests.Configuration
{

    [TestClass]
    public class ConfigurationFactoryTests
    {
        [TestMethod]
        public void CreateSettingsFromSingleFile()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsZipping
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.AreEqual(3, settings.Pipe.Length);
            Assert.AreEqual("From", settings.Pipe[0].StepName);
            Assert.AreEqual("Zip", settings.Pipe[1].StepName);
            Assert.AreEqual("To", settings.Pipe[2].StepName);

            Assert.IsNull(settings.Common);

            Assert.IsNotNull(settings.Pipe[0].StepSettings);
            Assert.IsNotNull(settings.Pipe[1].StepSettings);
            Assert.IsNotNull(settings.Pipe[2].StepSettings);
        }



        [TestMethod]
        public void CreateSettingsFromSingleFileAndSingleShared()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsZipping,
                CommonDataProvider.Configuration.SharedSettings
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.AreEqual(3, settings.Pipe.Length);
            Assert.AreEqual("From", settings.Pipe[0].StepName);
            Assert.AreEqual("Zip", settings.Pipe[1].StepName);
            Assert.AreEqual("To", settings.Pipe[2].StepName);

            Assert.AreEqual(2, settings.Common.Length);
            Assert.AreEqual("DefaultSource", settings.Common[0].Name);
            Assert.AreEqual("Test", settings.Common[1].Name);

            Assert.IsNotNull(settings.Pipe[0].StepSettings);
            Assert.IsNotNull(settings.Pipe[1].StepSettings);
            Assert.IsNotNull(settings.Pipe[2].StepSettings);

            Assert.IsNotNull(settings.Common[0].StepSettings);
            Assert.IsNotNull(settings.Common[1].StepSettings);
        }



        [TestMethod]
        public void CreateSettingsFromAlmostSameMultipleFiles()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsZipping,
                CommonDataProvider.Configuration.SettingsZippingWithPassword,
                CommonDataProvider.Configuration.SettingsZippingWithUpdatedPassword
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.AreEqual(3, settings.Pipe.Length);
            Assert.AreEqual("From", settings.Pipe[0].StepName);
            Assert.AreEqual("Zip", settings.Pipe[1].StepName);
            Assert.AreEqual("To", settings.Pipe[2].StepName);

            //Assert.AreEqual(    settings.Pipe[1].StepSettings);

            Assert.IsNull(settings.Common);

            Assert.IsNotNull(settings.Pipe[0].StepSettings);
            Assert.IsNotNull(settings.Pipe[1].StepSettings);
            Assert.IsNotNull(settings.Pipe[2].StepSettings);

            Assert.AreEqual("NEWER_PASSWORD", ((ZipSettings)settings.Pipe[1].StepSettings.ToObject(typeof(ZipSettings))).Password);
        }



        [TestMethod]
        public void CreateSettingsFromMultipleFileAndSingleShared()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsZipping,
                CommonDataProvider.Configuration.SettingsUnzipping,
                CommonDataProvider.Configuration.SharedSettings
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.AreEqual(3, settings.Pipe.Length);
            Assert.AreEqual("From", settings.Pipe[0].StepName);
            Assert.AreEqual("UnZip", settings.Pipe[1].StepName);
            Assert.AreEqual("To", settings.Pipe[2].StepName);

            Assert.AreEqual(2, settings.Common.Length);
            Assert.AreEqual("DefaultSource", settings.Common[0].Name);
            Assert.AreEqual("Test", settings.Common[1].Name);

            Assert.IsNotNull(settings.Pipe[0].StepSettings);
            Assert.IsNotNull(settings.Pipe[1].StepSettings);
            Assert.IsNotNull(settings.Pipe[2].StepSettings);

            Assert.IsNotNull(settings.Common[0].StepSettings);
            Assert.IsNotNull(settings.Common[1].StepSettings);
        }


        [TestMethod]
        public void CreateSettingsFromMultipleFileAndMultipleShared()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsZipping,
                CommonDataProvider.Configuration.SettingsUnzipping,
                CommonDataProvider.Configuration.SharedSettings,
                CommonDataProvider.Configuration.SharedSettings2
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.AreEqual(3, settings.Pipe.Length);
            Assert.AreEqual("From", settings.Pipe[0].StepName);
            Assert.AreEqual("UnZip", settings.Pipe[1].StepName);
            Assert.AreEqual("To", settings.Pipe[2].StepName);

            Assert.AreEqual(1, settings.Common.Length);
            Assert.AreEqual("Test00", settings.Common[0].Name);


            Assert.IsNotNull(settings.Pipe[0].StepSettings);
            Assert.IsNotNull(settings.Pipe[1].StepSettings);
            Assert.IsNotNull(settings.Pipe[2].StepSettings);

            Assert.IsNotNull(settings.Common[0].StepSettings);
        }

        [TestMethod]
        public void CreateWithoutConfigFiles()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();
           
            //Act
            Settings settings = factory.Get<Settings>(new List<FileInfo>());

            //Assert
            Assert.IsNotNull(settings);

        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSettingsWithNullFile()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            //Act
            Settings settings = factory.Get<Settings>(null);

            //Assert
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateSettingsWithNonExistingFile()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.NotExistingJSonFile
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);

            //Assert
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateSettingsWithNoJsonFile()
        {
            //Arrange
            IConfigurationFactory factory = new ConfigurationFactory();

            IEnumerable<FileInfo> testFiles = new FileInfo[]
            {
                CommonDataProvider.Configuration.SettingsInXml
            };

            //Act
            Settings settings = factory.Get<Settings>(testFiles);
        }

    }
}
