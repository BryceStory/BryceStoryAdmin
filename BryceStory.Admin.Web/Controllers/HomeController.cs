using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BryceStory.Admin.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using BryceStory.Utility;

namespace BryceStory.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (GlobalContext.SystemConfig.Demo)
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "123456";
            }
            return View();
        }
    }
}
