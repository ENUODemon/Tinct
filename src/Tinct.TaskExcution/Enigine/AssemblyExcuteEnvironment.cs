using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TaskExcution.Enigine
{
    public class AssemblyExcuteEnvironment
    {

        public ConcurrentDictionary<string, AppDomain> AppDomainDicts
        { get; private set; } = new ConcurrentDictionary<string, AppDomain>();


        public static AssemblyExcuteEnvironment Current { get; private set; }
        static AssemblyExcuteEnvironment()
        {
            Current = new AssemblyExcuteEnvironment();
        }

        public  void UnloadDomain(string key)
        {

            AppDomainDicts.TryRemove(key, out AppDomain targeDomain);
            if (targeDomain != null)
            {
                AppDomain.Unload(targeDomain);
            }
        }

    }
}
