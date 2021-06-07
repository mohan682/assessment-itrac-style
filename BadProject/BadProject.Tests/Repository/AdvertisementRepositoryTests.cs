using BadProject.Helpers;
using BadProject.Repository;
using NUnit.Framework;
using System.Threading;
using ThirdParty;

namespace BadProject.Tests.Repository
{
    class AdvertisementRepositoryTests
    {
        IAdvertisementRepository _advertisementRepository;

        [SetUp]
        public void Setup()
        {
            _advertisementRepository = new AdvertisementRepository();
        }

        [Test]
        public void GetAdvertisementUsingSqlProviderTest()
        {
            //Arrange
            string webId="myWebId";

            //Action 
            Advertisement  actualResult = _advertisementRepository.Get(Eums.ProviderTypes.Sql, webId);

            //Assert
            Assert.AreEqual(webId, actualResult.WebId);
            Assert.AreEqual($"Advertisement #{webId}", actualResult.Name);
        }

        [Test]
        public void GetAdvertisementUsingNoSqlProviderTest()
        {
            //Arrange
            string webId = "myWebId";

            //Action 
            Advertisement actualResult = _advertisementRepository.Get(Eums.ProviderTypes.NoSql, webId);

            //Assert
            Assert.AreEqual(webId, actualResult.WebId);
            Assert.AreEqual($"Advertisement #{webId}", actualResult.Name);
        }       
    }
}
