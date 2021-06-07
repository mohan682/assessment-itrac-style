using BadProject.Eums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Helpers
{
    public interface ICacheHelper 
    {
        bool IsExists(string key);
        T Get<T>(string key);
        void Set<T>(string key, T model);
        void Remove(string key);
    }
}
