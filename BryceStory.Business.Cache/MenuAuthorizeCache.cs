using BryceStory.Cache.Factory;
using BryceStory.Entity.SystemManage;
using BryceStory.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BryceStory.Business.Cache
{
    public class MenuAuthorizeCache : BaseBusinessCache<MenuAuthorizeEntity>
    {
        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<MenuAuthorizeEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuAuthorizeEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuAuthorizeService.GetList(null);
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
