using System;
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
namespace EBS.Admin.Controllers
{
    [Permission]
    public class HomeController : Controller
    {
        IQuery _query;
        public HomeController(IQuery query)
        {
            this._query = query;
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
            

            return View();
        }

        public ActionResult MenuTree()
        {
            // load menu,加载当前用户的
            //  
            var menus = _query.FindAll<Menu>(m => m.UrlType == Domain.ValueObject.MenuUrlType.MenuLink);
            StringBuilder builder = new StringBuilder();
            /* 格式如下
             *  <li class="treeview">
                        <a href="#">
                            <i class="fa fa-dashboard"></i> <span>Dashboard</span>
                            <span class="pull-right-container">
                                <i class="fa fa-angle-left pull-right"></i>
                            </span>
                        </a>
                        <ul class="treeview-menu">
                            <li><a href="../../index.html"><i class="fa fa-circle-o"></i> Dashboard v1</a></li>
                            <li><a href="../../index2.html"><i class="fa fa-circle-o"></i> Dashboard v2</a></li>
                        </ul>
                    </li>
             * 
             */
            
            foreach (var topMenu in menus.Where(m => m.ParentId == 0))
            {
                builder.AppendFormat("<li class=\"treeview\"><a href=\"javascript:showTab('{0}','{1}')\"><i class=\"fa fa-dashboard\"></i><span>{0}</span><span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span></a>",topMenu.Name, topMenu.Url );
                AddChildMenu(builder, topMenu, menus);
                builder.Append("</li>");    
            }
            ViewBag.Menus = builder.ToString();
            return PartialView();
        }

        public void AddChildMenu(StringBuilder builder,Menu parentMenu,IEnumerable<Menu> menus)
        {
            var children = menus.Where(m => m.ParentId == parentMenu.Id);
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