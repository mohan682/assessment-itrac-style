using ThirdParty;

namespace Adv.Services
{

    public interface IAdvertisementService
    {      
        bool ShouldTryreadFromCache { get; set; }
        bool ShouldTryBackupProvider { get; set; }
        int ErrorCount { get; }
        Advertisement GetAdvertisement(string id);
    }  
}
