using BadProject.Eums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Repository
{
    public interface IAdvertisementRepository
    {
        Advertisement Get(ProviderTypes providerType, string id);

    }
}
