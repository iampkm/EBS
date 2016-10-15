using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application;
using EBS.Domain.Entity;
using Dapper.DBContext;
using EBS.Application.DTO;
using EBS.Domain.Service;
using EBS.Domain.ValueObject;
namespace EBS.Application.Facade
{
    public class MenuFacade : IMenuFacade
    {
        IDBContext _db;
        MenuService _menuService;
        public MenuFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _menuService = new MenuService(this._db);
        }
        public void Create(MenuModel model)
        {
            model.Validate();
            Menu menu = new Menu(model.Name, model.Url, model.Icon, model.ParentId, model.DisplayOrder,(MenuUrlType)model.UrlType);
            _menuService.Create(menu);
        }

        public void Edit(MenuModel model)
        {
            model.Validate();
            Menu menu = new Menu(model.Name, model.Url, model.Icon, model.ParentId, model.DisplayOrder,(MenuUrlType)model.UrlType,model.Id);
            _menuService.Update(menu);
        }
        public void Delete(string ids)
        {
            _menuService.Delete(ids);
        }
    }
}
