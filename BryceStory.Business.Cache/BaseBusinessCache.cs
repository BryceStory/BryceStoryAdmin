using BryceStory.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BryceStory.Business.Cache
{
    public abstract class BaseBusinessCache<T>
    {
        public abstract string CacheKey { get; }

        public virtual bool Remove()
        {
            return CacheFactory.Cache.RemoveCache(CacheKey);
        }

        public virtual Task<List<T>> GetList()
        {
            throw new Exception("请在子类实现");
        }
    }
}
