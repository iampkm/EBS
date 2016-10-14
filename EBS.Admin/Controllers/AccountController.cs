using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Admin.Services;
using EBS.Application;
using EBS.Application.DTO;
using Newtonsoft.Json;
namespace EBS.Admin.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountFacade _accountFacade;
        public AccountController(IAuthenticationService authenticationService, IAccountFacade accountFacade)
        {
            this._authenticationService = authenticationService;
            this._accountFacade = accountFacade;
        }
        //
        // GET: /Account/
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            model.IpAddress = Request.UserHostAddress;
            var account = this._accountFacade.Login(model);
            var accountInfo= JsonConvert.SerializeObject(account);
            this._authenticationService.SignIn(account.UserName, accountInfo, model.RememberMe);
           // return RedirectToAction("DashBoard", "Home"); 
            return Json(new { success = true, returnUrl = "/Home/DashBoard" });
        }

        public ActionResult LogOut()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}