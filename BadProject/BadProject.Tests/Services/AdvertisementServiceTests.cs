using Adv.Services;
using BadProject.Helpers;
using BadProject.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using ThirdParty;

namespace BadProject.Tests.Services
{
    class AdvertisementServiceTests
    {
        IAdvertisementService _advertisementService;
        Mock<ICacheHelper> _mockCacheHelperType;
        Mock<IAdvertisementRepository> _mockAdvertisementRepositoryType;

       [SetUp]
        public void Setup()
        {
            _mockCacheHelperType = new Mock<ICacheHelper>();
            _mockAdvertisementRepositoryType = new Mock<IAdvertisementRepository>();
            _advertisementService = new AdvertisementService(_mockCacheHelperType.Object, _mockAdvertisementRepositoryType.Object);
        }

        [Test]
        public void GetAdvertisementFromCacheTest()
        {
            //Arrange
            string webId = "myWebId";

            Advertisement tempModel = new Advertisement { WebId=webId,Name=$"test {webId}"};

            _advertisementService.ShouldTryreadFromCache = true;
            _mockCacheHelperType.Setup(c => c.IsExists(It.IsAny<string>())).Returns(true);
            _mockCacheHelperType.Setup(c => c.Get<Advertisement>(It.IsAny<string>())).Returns(tempModel);

            //Action 
            Advertisement actualResult = _advertisementService.GetAdvertisement(webId);

            //Assert
            Assert.AreEqual(0, _advertisementService.ErrorCount);
            Assert.AreEqual(tempModel, actualResult);
        }

        [Test]
        public void GetAdvertisementSqlTest()
        {
            //Arrange
            string webId = "myWebId";

            Advertisement tempModel = new Advertisement { WebId = webId, Name = $"test {webId}" };

            _advertisementService.ShouldTryreadFromCache = false;

            _mockAdvertisementRepositoryType.Setup(a=>a.Get(Eums.ProviderTypes.Sql,webId)).Returns(tempModel);

            //Action 
            Advertisement actualResult = _advertisementService.GetAdvertisement(webId);

            //Assert
            Assert.AreEqual(0, _advertisementService.ErrorCount);
            Assert.AreEqual(tempModel, actualResult);
        }

        [Test]
        public void GetAdvertisementNoSqlTest()
        {
            //Arrange
            string webId = "myWebId";

            Advertisement tempModel = new Advertisement { WebId = webId, Name = $"test {webId}" };

            _advertisementService.ShouldTryreadFromCache = false;

            _mockAdvertisementRepositoryType.Setup(a => a.Get(Eums.ProviderTypes.Sql, webId)).Throws<Exception>();

            _mockAdvertisementRepositoryType.Setup(a => a.Get(Eums.ProviderTypes.NoSql, webId)).Returns(tempModel);

            //Action 
            Advertisement actualResult = _advertisementService.GetAdvertisement(webId);

            //Assert
            Assert.AreEqual(3, _advertisementService.ErrorCount);
            Assert.AreEqual(tempModel, actualResult);
        }
    }
}
