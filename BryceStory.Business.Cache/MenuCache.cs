using BryceStory.Cache.Factory;
using BryceStory.Entity.SystemManage;
using BryceStory.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BryceStory.Business.Cache
{
    public class MenuCache : BaseBusinessCache<MenuEntity>
    {
        private MenuService menuService = new MenuService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<MenuEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
