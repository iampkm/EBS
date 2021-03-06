﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBS.Infrastructure;
using EBS.Infrastructure.Log;
using EBS.Admin.Services;
using Dapper.DBContext;
using EBS.Domain.Entity;
using System.Text;
using EBS.Query;
namespace EBS.Admin.Controllers
{
    [Permission]
    public class HomeController : Controller
    {
        IQuery _query;
        IContextService _context;
        IMenuQuery _menuQuery;
        public HomeController(IContextService context,IQuery query,IMenuQuery menuQuery)
        {
            this._query = query;
            _context = context;
            _menuQuery = menuQuery;
        }
        public ActionResult Index()
        {
           // AppContext.Current.Resolve<ILogger>().Info("app context had Start  this is /home/index");
            return View();
        }

        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult DashBoard()
        {
            ViewBag.UserName = _context.CurrentAccount.NickName;
            ViewBag.RoleName = _context.CurrentAccount.RoleName;
            ViewBag.StoreName = string.IsNullOrEmpty(_context.CurrentAccount.StoreName) ? "总公司" : _context.CurrentAccount.StoreName;
            return View();
        }
       

        private string loadMenu()
        {
            // load menu,加载当前用户的
            //  
            var menus = _menuQuery.LoadMenu(_context.CurrentAccount.RoleId).Where(n => n.UrlType == Domain.ValueObject.MenuUrlType.MenuLink);
            if (!menus.Any()) {
                //没有分配菜单，加载所有
                menus = _query.FindAll<Menu>(m => m.UrlType == Domain.ValueObject.MenuUrlType.MenuLink); 
            }
           // var menus = _query.FindAll<Menu>(m => m.UrlType == Domain.ValueObject.MenuUrlType.MenuLink);
            StringBuilder builder = new StringBuilder();            

            foreach (var topMenu in menus.Where(m => m.ParentId == 0).OrderBy(n=>n.DisplayOrder).ToList())
            {
                builder.AppendFormat("<li class=\"treeview\"><a href=\"javascript:showTab('{0}','{1}')\"><i class=\"fa {2}\"></i><span>{0}</span><span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span></a>", topMenu.Name, topMenu.Url,topMenu.Icon);
                AddChildMenu(builder, topMenu, menus);
                builder.Append("</li>");
            }
           return builder.ToString();
        }

        public ActionResult MenuTree()
        {
            ViewBag.Menus = loadMenu();
            return PartialView();
        }

        public void AddChildMenu(StringBuilder builder,Menu parentMenu,IEnumerable<Menu> menus)
        {
            var children = menus.Where(m => m.ParentId == parentMenu.Id).OrderBy(n => n.DisplayOrder).ToList();
            if(children.Count()==0){ return ;}
            builder.Append("<ul class=\"treeview-menu\">");
            foreach (var child in children)
            {
                builder.AppendFormat("<li><a href=\"javascript:showTab('{0}','{1}')\" ><i class=\"fa fa-circle-o\"></i>{0}</a>",  child.Name,child.Url);
                AddChildMenu(builder, child, menus);
                builder.Append("</li>");
            }
            builder.Append("</ul>");    
        }

        private void loadError()
        {
            throw new Exception("code is error");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}