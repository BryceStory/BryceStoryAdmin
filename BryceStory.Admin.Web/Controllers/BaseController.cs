using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BryceStory.Utility.Extention;
using BryceStory.Web.Code;
using BryceStory.Utility;
using BryceStory.Utility.Model;

namespace BryceStory.Admin.Web.Controllers
{
    /// <summary>
    /// 基础控制器，用来记录访问日志
    /// </summary>
    public class BaseController : Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
                
            string action = context.RouteData.Values["Action"].ParseToString();

            OperatorInfo user = await Operator.Instance.Current();

            if (GlobalContext.SystemConfig.Demo)
            {
                if (context.HttpContext.Request.Method.ToUpper()=="POST")
                {
                    string[] allowAction = new string[] { "LoginJson", "ExportUserJson", "CodePreviewJson" };
                    if (!allowAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
                    {
                        TData obj = new TData();
                        obj.Message = "演示模式，不允许操作";
                        context.Result = new JsonResult(obj);
                        return;
                    }
                }
            }


        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
