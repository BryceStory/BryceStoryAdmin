using BryceStory.Cache.Interface;
using System;
using BryceStory.Utility;
using BryceStory.RedisCache;
using BryceStory.MemoryCache;

namespace BryceStory.Cache.Factory
{
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();


        public static ICache Cache
        { 
        get
            {
                if (cache==null)
                {
                    lock (lockHelper)
                    {
                        if (cache==null)
                        {
                            switch (GlobalContext.SystemConfig.CacheProvider)
                            {
                                case "Redis": cache = new RedisCacheImp(); break;

                                default:
                                case "Memory": cache = new MemoryCacheImp(); break;
                            }
                        }
                    }
                }
                return cache;
            }
        }
    }
}
