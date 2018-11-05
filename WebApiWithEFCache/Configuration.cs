using EFCache;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace WebApiWithEFCache
{
    public class CustomConfiguration : DbConfiguration
    {

        public class CachingPolicySelected : CachingPolicy
        {

            protected override bool CanBeCached(ReadOnlyCollection<EntitySetBase> affectedEntitySets, string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            {

                //cacheamos resultados ConfigurationDAL
                Dictionary<string, List<string>> cacheList = new Dictionary<string, List<string>>(){
                    {"ModelStoreContainer", new List<string>(){ "Aircraft"}}
                };

                if (affectedEntitySets.Count == 1)
                {
                    var containerStoreName = affectedEntitySets.FirstOrDefault().EntityContainer.Name;
                    var setName = affectedEntitySets.FirstOrDefault().Name;
                    return cacheList.ContainsKey(containerStoreName) && cacheList[containerStoreName].Contains(setName);
                }
                return false;

            }

        }

        public CustomConfiguration()
        {
            //var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

            //AddInterceptor(transactionHandler);

            //Loaded +=
            //  (sender, args) => args.ReplaceService<DbProviderServices>(
            //    (s, _) => new CachingProviderServices(s, transactionHandler,
            //      new CachingPolicy()));


            // Somehow Custom CachingPolicy not working
            var tablesToCache = new List<string> { "Aircraft" };

            var transactionHandler = new CacheTransactionHandler(WebApiApplication.Cache);

            AddInterceptor(transactionHandler);

            Loaded +=
              (sender, args) => args.ReplaceService<DbProviderServices>(
                (s, _) => new CachingProviderServices(s, transactionHandler, new CachingPolicySelected()));
        }

    }
}