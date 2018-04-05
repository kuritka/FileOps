using System;
using Beamer.Infrastructure.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileOps.Processors.Compression;
using FileOps.Configuration.Entities;

namespace Beamer.Tests.Compression
{

    [TestClass]
    public class ZipFactoryTests
    {

        [TestMethod]
        public void GetZipPasswordProtectedInstance()
        {
            //arrange
            IZipFactory zipFactory = new ZipFactory(new ZipSettings { Password = "some.Password" });
            //act
            ICompressor instance =  zipFactory.Get();
            //assert
            Assert.AreEqual(typeof(PasswordProtectedZip), instance.GetType());
        }


        [TestMethod]
        public void GetZipInstance()
        {
            //arrange
            IZipFactory zipFactory = new ZipFactory(new ZipSettings { Password = string.Empty });
            //act
            ICompressor instance = zipFactory.Get();
            //assert
            Assert.AreEqual(typeof(Zip), instance.GetType());
        }


        [TestMethod]
        [ExpectedExceptionAttribute(typeof(ArgumentNullException))]
        public void CreateInstanceWithoutZipSettings()
        {
            //arrange
            //act
            //assert
            IZipFactory zipFactory = new ZipFactory(null);
        }
    }
}
