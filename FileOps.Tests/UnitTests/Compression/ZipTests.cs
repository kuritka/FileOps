//using Beamer.Entities;
//using Beamer.Entities.PipeSettings;
//using Beamer.Infrastructure.Pipe.Steps;
//using Beamer.Utils.TestExtensions;
//using FakeItEasy;
//using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.IO;

//namespace Beamer.Tests.Compression
//{
//    [TestClass]
//    public class ZipTests
//    {
//        private readonly ApplicationSettings _applicationSettings = new ApplicationSettings
//        {
//            MongoSettings = new MongoSettings()
//        };
//        private readonly DirectoryInfo _workingDirectory = new DirectoryInfo("ZipTest");
//        private readonly ILoggerFactory _loggerFactory = A.Fake<ILoggerFactory>();
//        private readonly ZipSettings _zipSettings = new ZipSettings();

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "zipSettings")]
//        public void ZipZipSettingsIsNull()
//        {
//            Zip unused = new Zip(null, _applicationSettings, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "applicationSettings")]
//        public void ZipApplicationSettingsIsNull()
//        {
//            Zip unused = new Zip(_zipSettings, null, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "MongoSettings")]
//        public void ZipMongoSettingsIsNull()
//        {
//            _applicationSettings.MongoSettings = null;
//            Zip unused = new Zip(_zipSettings, _applicationSettings, _workingDirectory, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "parentWorkingDirectory")]
//        public void ZipParentWorkingDirectoryIsNull()
//        {
//            Zip unused = new Zip(_zipSettings, _applicationSettings, null, _loggerFactory);
//        }

//        [TestMethod]
//        [ExtendedExpectedException(typeof(ArgumentNullException), ParameterName = "loggerFactory")]
//        public void ZipLoggerFactoryIsNull()
//        {
//            Zip unused = new Zip(_zipSettings, _applicationSettings, _workingDirectory, null);
//        }
//    }
//}