using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Admin.Services;
using EBS.Application;
using EBS.Application.DTO;
using Newtonsoft.Json;
using EBS.Query;
using EBS.Query.DTO;
using EBS.Domain.Entity;
using Dapper.DBContext;
using EBS.Infrastructure;
namespace EBS.Admin.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountFacade _accountFacade;
        private IAccountQuery _accountQuery;
        private IQuery _query;
        public AccountController(IAuthenticationService authenticationService, IQuery query, IAccountFacade accountFacade, IAccountQuery accountQuery)
        {
            this._authenticationService = authenticationService;
            this._query = query;
            this._accountFacade = accountFacade;
            this._accountQuery = accountQuery;
        }
        [Permission]
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult LoadData(Pager page, int? id, string userName, string nickName)
        {
            var rows = _accountQuery.GetPageList(page, id, userName, nickName);
            return Json(new { success = true, data = rows, total = page.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            model.IpAddress = Request.UserHostAddress;
            var account = this._accountFacade.Login(model);
            var accountInfo = JsonConvert.SerializeObject(account);
            this._authenticationService.SignIn(account.UserName, accountInfo, model.RememberMe);
            // return RedirectToAction("DashBoard", "Home"); 
            return Json(new { success = true, returnUrl = "/Home/DashBoard" });
        }

        public ActionResult LogOut()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Permission]
        public ActionResult Create()
        {
            //加载权限资源          
            ViewBag.Roles = _query.FindAll<Role>();
            return View();
        }
        [HttpPost]
        public JsonResult Create(CreateAccountModel model)
        {
            _accountFacade.Create(model);
            return Json(new { success = true });
        }
        [Permission]
        public ActionResult Edit(int id)
        {
            var model = _query.Find<Account>(id);
            ViewBag.Roles = _query.FindAll<Role>();
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(EditAccountModel model)
        {
            _accountFacade.Edit(model);
            return Json(new { success = true });
        }

        public JsonResult Actived(int id)
        {
            _accountFacade.ActiveAccount(id);
            return Json(new { success = true });
        }

        public JsonResult Disabled(int id)
        {
            _accountFacade.DisabledAccount(id);
            return Json(new { success = true });
        }

        public JsonResult ResetPassword(int id)
        {
            _accountFacade.ResetPassword(id);
            return Json(new { success = true });
        }
        [Permission]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            // 获取当前用户
            var contextService= AppContext.Current.Resolve<IContextService>();
            _accountFacade.ChangePassword(contextService.CurrentAccount.AccountId, oldPassword, newPassword);
            return Json(new { success = true });
        }
    }
}