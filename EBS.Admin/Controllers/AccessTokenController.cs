using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.DBContext;
using EBS.Domain.Entity;
using EBS.Application;
using EBS.Application.DTO;
using EBS.Query.DTO;
using EBS.Query;
using EBS.Admin.Services;
namespace EBS.Admin.Controllers
{
    public class AccessTokenController : Controller
    {
        IContextService _context;
        IDBContext _db;
        IQuery _iquery;
        IPosSyncFacade _posFacade;
        IAccessTokenFacade _accessTokenFacade;
        IAccessTokenQuery _accessTokenQuery;
        public AccessTokenController(IContextService context, IDBContext db, IQuery iquery, IPosSyncFacade posFacade, IAccessTokenFacade accessTokenFacade, IAccessTokenQuery accessTokenQuery)
        {
            _context = context;
            _db = db;
            _iquery = iquery;
            _posFacade = posFacade;
            _accessTokenFacade = accessTokenFacade;
            _accessTokenQuery = accessTokenQuery;
        }
        //
        // GET: /AccessToken/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData(Pager page, int storeId)
        {
            var rows = _accessTokenQuery.GetPageList(page, storeId);
            return Json(new { success = true, data = rows, total = page.Total });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(AccessTokenModel model)
        {
            _accessTokenFacade.Create(model);
            return Json(new { success = true,data = model });
        }


	}
}