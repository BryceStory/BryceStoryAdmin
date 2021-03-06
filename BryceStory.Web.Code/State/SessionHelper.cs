﻿using System;
using System.Collections.Generic;
using System.Text;
using BryceStory.Utility;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace BryceStory.Web.Code.State
{
    public class SessionHelper
    {
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.SetString(key, JsonConvert.SerializeObject(value));

        }

        /// <summary>
        /// 写Session    
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteSession(string key, string value)
        {
            WriteSession<string>(key,value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Session.GetString(key) as string;
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.Remove(key);
        }
    }
}
