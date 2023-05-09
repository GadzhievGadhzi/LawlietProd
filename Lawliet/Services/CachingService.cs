using Lawliet.Data;
using Lawliet.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Lawliet.Services {
    public class CachingService {
        private UserDataContext context;
        private IMemoryCache cache;

        public CachingService(UserDataContext context, IMemoryCache cache) {
            this.context = context;
            this.cache = cache;
        }

        public async Task AddObjectFromCache<TEntity>(TEntity user, MemoryCacheEntryOptions? options = null) where TEntity : class, IDataModel {
            DataRepository<TEntity> repository = new DataRepository<TEntity>(context);
            var _user = repository.Get(search => search.Id == user.Id).FirstOrDefault();
            if (_user == null) {
                await repository.CreateAsync(user);
                cache.Set(user.Id, user, options);
            } else {
                cache.Set(user.Id, _user, options);
            }
        }

        public TEntity GetObjectFromCache<TEntity>(string id, MemoryCacheEntryOptions? options = null) where TEntity : class, IDataModel {
            TEntity? user;
            if (!cache.TryGetValue(id, out user)) {
                DataRepository<TEntity> repository = new DataRepository<TEntity>(context);
                user = repository.Get(search => search.Id == id).FirstOrDefault();
                if (user != null) {
                    cache.Set(user.Id, user, options);
                }
            }

            return user;
        }

        public void UpdateObject<TEntity>(TEntity item) where TEntity : class, IDataModel {
            DataRepository<TEntity> repository = new DataRepository<TEntity>(context);
            repository.Update(item);

            cache.Remove(item.Id);
            AddObjectFromCache(item);
        }

        public void RemoveObjectFromCache(string id) {
            cache.Remove(id);
        }
    }
}