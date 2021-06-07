using BadProject;
using BadProject.Helpers;
using BadProject.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using System.Threading;
using ThirdParty;

namespace Adv.Services
{

    public class AdvertisementService : IAdvertisementService
    {
        private static Queue<DateTime> errors = new Queue<DateTime>();

        private Object lockObj = new Object();

        readonly IAdvertisementRepository _advRepository;
        readonly ICacheHelper _cacheHelper;
        readonly RetryHelper _retryHelper;
        private static readonly int _retryWaitInSeconds = int.TryParse(ConfigurationManager.AppSettings["RetryWaitInSeconds"], out int retryWaitInSeconds) ? retryWaitInSeconds : 1000;
        private static readonly int _retryCount = int.TryParse(ConfigurationManager.AppSettings["RetryCount"], out int retryCount) ? retryCount : 3;

        public bool ShouldTryreadFromCache { get; set; } = true;
        public bool ShouldTryBackupProvider { get; set; } = true;
        public int ErrorCount => errors.Count;

        public AdvertisementService(ICacheHelper cacheHelper, IAdvertisementRepository advRepository) :
            this(cacheHelper, advRepository,
                _retryWaitInSeconds,
                _retryCount)
        {

        }

        public AdvertisementService(ICacheHelper cacheHelper, IAdvertisementRepository advRepository, int retryWaitInSeconds, int retryCount)
        {
            _advRepository = advRepository;

            _cacheHelper = cacheHelper;

            _retryHelper = new RetryHelper(retryWaitInSeconds, retryCount);
        }

        public Advertisement GetAdvertisement(string id)
        {
            Advertisement adv = null;

            lock (lockObj)
            {
                string cacheKey = $"AdvKey_{id}";

                if (ShouldTryreadFromCache && _cacheHelper.IsExists(cacheKey))
                {
                    return _cacheHelper.Get<Advertisement>(cacheKey);
                }

                RemoveErrorMoreThanSpecifiedHours(DateTime.Now.AddHours(-1));

                if (ErrorCount < 10)
                {
                    adv = GetAdvertisement(id, adv, cacheKey);
                }

                if (ShouldTryBackupProvider && adv == null)
                {
                    adv = _advRepository.Get(BadProject.Eums.ProviderTypes.NoSql, id);

                    AddToCache(adv, cacheKey);
                }
            }

            return adv;
        }

        private Advertisement GetAdvertisement(string id, Advertisement adv, string cacheKey)
        {
            _retryHelper.Execute(
                () => _advRepository.Get(BadProject.Eums.ProviderTypes.Sql, id),
                (Advertisement advertisement) =>
                {
                    adv = advertisement;

                    AddToCache(advertisement, cacheKey);
                },
                (Exception ex) =>
                {
                    errors.Enqueue(DateTime.Now);
                });


            if (adv != null)
            {
                _cacheHelper.Set(cacheKey, adv);
            }

            return adv;
        }

        private void RemoveErrorMoreThanSpecifiedHours(DateTime targetTimeToExpiry)
        {
            while (errors.Count > 0 && errors.Peek() > targetTimeToExpiry)
            {
                errors.Dequeue();
            }
        }

        private void AddToCache(Advertisement advertisement, string cacheKey)
        {
            if (advertisement != null)
            {
                _cacheHelper.Set(cacheKey, advertisement);
            }
        }
    }
}
