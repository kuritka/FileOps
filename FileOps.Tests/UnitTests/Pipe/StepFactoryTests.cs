using FileOps.Common;
using FileOps.Configuration;
using FileOps.Configuration.Entities;
using FileOps.Pipe;
using FileOps.Tests.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOps.Tests.UnitTests.Pipe
{
    [TestClass]
    public class StepFactoryTests
    {

        [TestMethod]
        public void GetStepsForConfigFileOnly()
        {
            //Arrange
            Settings settings = new ConfigurationFactory()
                .Get(FileProvider.Configuration.SettingsZipping.AsOneItemList());

            //Act
            IEnumerable<IStep> steps = new StepFactory().Get(settings);

            //Assert
            Assert.AreEqual(3, steps.Count());
        }


        [TestMethod]
        public void GetStepsFromMultipleSharedFilesAndMultipleConfigFiles()
        {
            //Arrange
            FileInfo[] configuration = new FileInfo[]
            {
                FileProvider.Configuration.SettingsZipping,
                FileProvider.Configuration.SettingsUnzipping,
                FileProvider.Configuration.SharedSettings,
                FileProvider.Configuration.SharedSettings2
            };

            Settings settings = new ConfigurationFactory()
                .Get(configuration);

            //Act
            IEnumerable<IStep> steps = new StepFactory().Get(settings);

            //Assert
            Assert.AreEqual(3, steps.Count());
        }


        [TestMethod]
        public void LoadOnlySharedConfiguration()
        {
            //Arrange
            Settings settings = new ConfigurationFactory()
                .Get(FileProvider.Configuration.SharedSettings.AsOneItemList());

            //Act
            IEnumerable<IStep> steps = new StepFactory().Get(settings);

            //Assert
            Assert.AreEqual(steps.Count(), 0);
        }




        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStepsWithNullSettings()
        {
            //Arrange
            //Act
            //Assert
            IEnumerable<IStep> steps = new StepFactory().Get(null);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStepsWithNullPipeSettings()
        {
            //Arrange
            //Act
            //Assert
            IEnumerable<IStep> steps = new StepFactory().Get(new Settings());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStepsWithEmptySettings()
        {
            //Arrange
            //Act
            //Assert
            IEnumerable<IStep> steps = new StepFactory().Get(new Settings());
        }


        [TestMethod]
        public void GetStepsWithReferences()
        {
            //Arrange
            FileInfo[] configFiles = new FileInfo[]
            {
                FileProvider.Configuration.SettingsZippingWithReferences,
                FileProvider.Configuration.SettingsZippingWithReferencesShared
            };

            Settings settings = new ConfigurationFactory()
                .Get(configFiles);

            //Act
            IEnumerable<IStep> steps = new StepFactory().Get(settings).ToList();

            //Assert
            Assert.AreEqual(3, steps.Count());
            
        }



    }
}
