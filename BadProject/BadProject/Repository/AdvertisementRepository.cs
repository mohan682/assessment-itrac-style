using BadProject.Eums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {

        NoSqlAdvProvider _noSqlAdvProvider;

        public AdvertisementRepository()
        {
            _noSqlAdvProvider = new NoSqlAdvProvider();
        }

        public Advertisement Get(ProviderTypes providerType, string id)
        {
            Advertisement adv = null;

            if (providerType == ProviderTypes.Sql)
            {
                adv = _noSqlAdvProvider.GetAdv(id);
            }
            else if (providerType == ProviderTypes.NoSql)
            {
                adv = SQLAdvProvider.GetAdv(id);
            }

            return adv;
        }
    }
}
