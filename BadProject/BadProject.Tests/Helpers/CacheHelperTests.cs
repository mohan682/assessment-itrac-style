using BadProject.Helpers;
using NUnit.Framework;
using System.Threading;

namespace BadProject.Tests.Helpers
{
    class CacheHelperTests
    {
        ICacheHelper _cacheHelper;

        [SetUp]
        public void Setup()
        {
            _cacheHelper = new CacheHelper(1);
        }

        [Test]
        public void SetAndGetValidTest()
        {
            //Arrange
            _cacheHelper.Set<string>("myKey", "Test String");

            //Action 
            string actualResult = _cacheHelper.Get<string>("myKey");

            //Assert
            Assert.AreEqual("Test String", actualResult);
        }

        [Test]
        public void SetAndItemExpiredTest()
        {
            //Arrange
            _cacheHelper.Set<string>("myKey", "Test String");

            Thread.Sleep(60001);
            
            //Action 
            string actualResult = _cacheHelper.Get<string>("myKey");

            //Assert
            Assert.AreEqual(default(string), actualResult);
        }

        [TearDown]
        public void TearDown()
        {
            _cacheHelper.Remove("myKey");
        }
    }


}
