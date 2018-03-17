using FileOps.Steps.From;
using FileOps.Steps.To;
using FileOps.Steps.UnZip;
using FileOps.Steps.Zip;
using FileOps.Tests.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace FileOps.Tests.UnitTests.Pipe
{

    [TestClass]
    public class FileOpsBuilderTests
    {

        [TestMethod]
        public void CreateConfigurationFromSingleFile()
        {
            //Arrange
            //Act
            var steps = new FileOpsBuilder()
              .AddConfiguration(CommonDataProvider.Configuration.SettingsZipping)
              .Build().ToList();

            //Assert
            Assert.AreEqual(3, steps.Count);
            Assert.AreEqual(typeof(From).Name, steps[0].GetType().Name);
            Assert.AreEqual(typeof(Zip).Name, steps[1].GetType().Name);
            Assert.AreEqual(typeof(To).Name, steps[2].GetType().Name);
        }


        [TestMethod]
        public void CreateConfigurationFromSingleFileAndSingleShared()
        {
            //Arrange
            //Act
            var steps = new FileOpsBuilder()
              .AddConfiguration(CommonDataProvider.Configuration.SettingsZipping)
               .AddConfiguration(CommonDataProvider.Configuration.SharedSettings)
              .Build().ToList();

            //Assert
            Assert.AreEqual(3, steps.Count);
            Assert.AreEqual(typeof(From).Name, steps[0].GetType().Name);
            Assert.AreEqual(typeof(Zip).Name, steps[1].GetType().Name);
            Assert.AreEqual(typeof(To).Name, steps[2].GetType().Name);
        }

        [TestMethod]
        public void CreateConfigurationFromAlmostSameMultipleFiles()
        {
            //Arrange
            //Act
            var steps = new FileOpsBuilder()
              .AddConfiguration(CommonDataProvider.Configuration.SettingsZipping)
               .AddConfiguration(CommonDataProvider.Configuration.SettingsZippingWithPassword)
               .AddConfiguration(CommonDataProvider.Configuration.SettingsZippingWithUpdatedPassword)
              .Build().ToList();

            //Assert
            Assert.AreEqual(3, steps.Count);
            Assert.AreEqual(typeof(From).Name, steps[0].GetType().Name);
            Assert.AreEqual(typeof(Zip).Name, steps[1].GetType().Name);
            Assert.AreEqual(typeof(To).Name, steps[2].GetType().Name);
        }


        [TestMethod]
        public void CreateConfigurationFromMultipleFileAndSingleShared()
        {
            //Arrange
            //Act
            var steps = new FileOpsBuilder()
              .AddConfiguration(CommonDataProvider.Configuration.SettingsZipping)
               .AddConfiguration(CommonDataProvider.Configuration.SettingsUnzipping)
               .AddConfiguration(CommonDataProvider.Configuration.SharedSettings)
              .Build().ToList();

            //Assert
            Assert.AreEqual(3, steps.Count);
            Assert.AreEqual(typeof(From).Name, steps[0].GetType().Name);
            Assert.AreEqual(typeof(UnZip).Name, steps[1].GetType().Name);
            Assert.AreEqual(typeof(To).Name, steps[2].GetType().Name);
        }


        [TestMethod]
        public void CreateFromMultipleFileAndMultipleShared()
        {
            //Arrange
            //Act
            var steps = new FileOpsBuilder()
              .AddConfiguration(CommonDataProvider.Configuration.SettingsZipping)
               .AddConfiguration(CommonDataProvider.Configuration.SettingsUnzipping)
               .AddConfiguration(CommonDataProvider.Configuration.SharedSettings)
               .AddConfiguration(CommonDataProvider.Configuration.SharedSettings2)
              .Build().ToList();

            //Assert
            Assert.AreEqual(3, steps.Count);
            Assert.AreEqual(typeof(From).Name, steps[0].GetType().Name);
            Assert.AreEqual(typeof(UnZip).Name, steps[1].GetType().Name);
            Assert.AreEqual(typeof(To).Name, steps[2].GetType().Name);
        }

        [TestMethod]
        public void CreateWithoutConfigFiles()
        {
            //Arrange
            //Act
            //Assert
            var steps = new FileOpsBuilder()
            .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullFile()
        {
            //Arrange
            //Act
            //Assert
            var steps = new FileOpsBuilder()
            .AddConfiguration(null)
            .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateWithNonExistingFile()
        {
            //Arrange
            //Act
            //Assert
            var steps = new FileOpsBuilder()
            .AddConfiguration(CommonDataProvider.Configuration.NotExistingJSonFile)
            .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateWithNoJsonFile()
        {
            //Arrange
            //Act
            //Assert
            var steps = new FileOpsBuilder()
            .AddConfiguration(CommonDataProvider.Configuration.SettingsInXml)
            .Build();
        }





    }
}
