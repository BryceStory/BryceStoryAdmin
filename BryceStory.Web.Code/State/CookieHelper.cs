using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using BryceStory.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace BryceStory.Web.Code.State
{
    public class CookieHelper
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sKey"></param>
        public void WriteCookie(string sName, string sValue)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(30);
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue);

        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public void WriteCookie(string sName, string sValue, int expires)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expires);
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public string GetCookie(string sName)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();

            return hca?.HttpContext?.Request.Cookies[sName];
        }


        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        public void RemoveCookie(string sName)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Response.Cookies.Delete(sName);
        }
    }
}
