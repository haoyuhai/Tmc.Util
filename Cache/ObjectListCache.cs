using System.Collections.Generic;
using System.Linq;

namespace Tmc.Util.Cache
{
    /// <summary>
    /// 集合对象的静态缓存实现基类
    /// </summary>
    /// <typeparam name="TEntity">缓存的实体对象的类型</typeparam>
    /// <typeparam name="TKey">对象Key的类型</typeparam>
    /// <typeparam name="TCache">当前的缓存类型</typeparam>
    public abstract class ObjectListCache<TEntity, TKey, TCache> : SingletonBase<TCache>
        where TCache : ObjectListCache<TEntity, TKey, TCache>
        where TEntity : class
    {
        private volatile bool initialized;
        private readonly object lockObj = new object();

        private List<TEntity> _Items;
        private Dictionary<TKey, TEntity> _ItemsDic;

        protected ObjectListCache()
        {
        }

        /// <summary>
        /// 根据指定的键值获取缓存的内部对象,请勿将更改此对象,否则会更改缓存对象
        /// </summary>
        public TEntity GetItemUnSafe(TKey key)
        {
            if (key == null) return null;

            AssertCacheItemsLoaded();

            //线程不安全，为性能考虑，这里未进行线程同步！
            TEntity result;
            _ItemsDic.TryGetValue(key, out result);
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
            AssertCacheItemsLoaded();

            //线程不安全，为性能考虑，这里未进行线程同步！            
            return _Items.ToList();
        }

        /// <summary>
        /// 获取所有的缓存项集合
        /// </summary>
        public virtual List<TEntity> GetAll()
        {
            AssertCacheItemsLoaded();
                        
            //为防止外部修改影响缓存，这里返回一个全新的实例
            return (List<TEntity>)_Items.Clone();
        }

        /// <summary>
        /// 清空缓存，下次将重新加载
        /// </summary>
        public virtual void Refresh()
        {
            lock (lockObj)
            {
                initialized = false;
                _Items = null;
                _ItemsDic = null;
            }
        }

        protected void AssertCacheItemsLoaded()
        {
            if (!initialized)
            {
                lock (lockObj)
                {
                    if (!initialized)
                    {
                        List<TEntity> items = LoadCacheItems();

                        Dictionary<TKey, TEntity> dic = new Dictionary<TKey, TEntity>();
                        foreach (TEntity item in items)
                        {
                            TKey key = GetKey(item);
                            if (!dic.ContainsKey(key))
                            {
                                dic.Add(key, item);
                            }
                        }

                        _Items = items;
                        _ItemsDic = dic;
                        initialized = true;
                    }
                }
            }
        }

        protected abstract List<TEntity> LoadCacheItems();
        protected abstract TKey GetKey(TEntity item);
    }
}
