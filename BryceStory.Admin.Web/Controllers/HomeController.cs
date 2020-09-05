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
using BryceStory.Web.Code;
using BryceStory.Enum;
using BryceStory.Utility.Model;
using BryceStory.Entity.SystemManage;
using BryceStory.Utility.Extention;
using BryceStory.Model.Result;
using BryceStory.Business.SystemManage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BryceStory.Web.Code.State;
using BryceStory.Business.OrganizationManage;

namespace BryceStory.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();
        private UserBLL userBLL = new UserBLL();
        private LogLoginBLL logLoginBLL = new LogLoginBLL();
        private MenuAuthorizeBLL menuAuthorizeBLL = new MenuAuthorizeBLL();



        [HttpGet]
       // [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            TData<List<MenuEntity>> objMenu = await menuBLL.GetList(null);
            List<MenuEntity> menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }



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

        #region 获取数据
        public IActionResult GetCaptchaImage()
        {
            string sessionId = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;

            Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            new SessionHelper().WriteSession("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region 提交数据
        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != new SessionHelper().GetSession("CaptchaCode").ParseToString())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }
            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = userObj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = userObj.Data?.Id
                };

                // 让底层不用获取HttpContext
                logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                await logLoginBLL.SaveForm(logLoginEntity);
            };
            AsyncTaskHelper.StartTask(taskAction);

            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }
        #endregion
    }
}
