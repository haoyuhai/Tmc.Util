using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tmc.Util.Cache
{
    [Serializable]
    internal class DynamicListCacheInner<TEntity, TKey>
    {
        public List<TEntity> Items { get; set; }
        public Dictionary<TKey, TEntity> ItemsDic { get; set; }
    }

    /// <summary>
    /// 集合对象动态缓存的实现基类
    /// </summary>
    /// <typeparam name="TEntity">缓存的实体对象的类型</typeparam>
    /// <typeparam name="TKey">对象Key的类型</typeparam>
    /// <typeparam name="TCache">缓存对象的类型(单实例)</typeparam>
    public abstract class ObjectListDynamicCache<TEntity, TKey, TCache> : SingletonBase<TCache>
        where TCache : ObjectListDynamicCache<TEntity, TKey, TCache>
        where TEntity : class
    {
        private readonly string _CacheKey = "DynamicCache_" + typeof(TCache).FullName;
        private readonly object _Lock = new object();

        protected ObjectListDynamicCache()
        {
        }

        /// <summary>
        /// 根据指定的键值获取缓存项
        /// </summary>
        public virtual TEntity GetItemUnSafe(TKey key)
        {
            if (key == null) return null;

            DynamicListCacheInner<TEntity, TKey> cacheObj = GetCacheObj();

            TEntity result;
            cacheObj.ItemsDic.TryGetValue(key, out result);            
            return result;
        }

        /// <summary>
        /// 根据指定的键值获取缓存项
        /// </summary>
        public virtual TEntity GetItem(TKey key)
        {
            TEntity result = GetItemUnSafe(key);
            if (result == null) return null;

            //为防止外部修改影响缓存，这里返回一个全新的实例
            return (TEntity)result.Clone();
        }

        /// <summary>
        /// 获取所有的缓存项集合
        /// </summary>
        public virtual List<TEntity> GetAllUnSafe()
        {
            DynamicListCacheInner<TEntity, TKey> cacheObj = GetCacheObj();
            return cacheObj.Items.ToList();
        }

        /// <summary>
        /// 获取所有的缓存项集合
        /// </summary>
        public virtual List<TEntity> GetAll()
        {
            DynamicListCacheInner<TEntity, TKey> cacheObj = GetCacheObj();
            //为防止外部修改影响缓存，这里返回一个全新的实例
            return (List<TEntity>)cacheObj.Items.Clone();
        }

        /// <summary>
        /// 清除缓存项，下次调用时将重新加载
        /// </summary>
        public virtual void Refresh()
        {
            HttpRuntime.Cache.Remove(_CacheKey);
        }

        private DynamicListCacheInner<TEntity, TKey> GetCacheObj()
        {
            var cacheObj = HttpRuntime.Cache.Get(_CacheKey) as DynamicListCacheInner<TEntity, TKey>;
            if (cacheObj != null) return cacheObj;

            lock (_Lock)
            {
                cacheObj = HttpRuntime.Cache.Get(_CacheKey) as DynamicListCacheInner<TEntity, TKey>;
                if (cacheObj != null) return cacheObj;

                List<TEntity> items = LoadCacheItems() ?? new List<TEntity>();

                cacheObj = new DynamicListCacheInner<TEntity, TKey>
                {
                    Items = items,
                    ItemsDic = GetItemsDic(items)
                };

                //加入动态缓存
                HttpRuntime.Cache.Insert(_CacheKey, cacheObj, null, DateTime.UtcNow.Add(GetCacheDuration()), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return cacheObj;
        }

        private Dictionary<TKey, TEntity> GetItemsDic(List<TEntity> items)
        {
            var dic = new Dictionary<TKey, TEntity>();
            foreach (var item in items)
            {
                TKey key = GetKey(item);
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, item);
                }
            }
            return dic;
        }

        protected abstract List<TEntity> LoadCacheItems();
        protected abstract TKey GetKey(TEntity item);
        protected abstract TimeSpan GetCacheDuration();
    }
}
