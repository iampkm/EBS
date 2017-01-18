using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
using Newtonsoft.Json;
using EBS.Domain.Entity;
using Dapper.DBContext;
using EBS.Domain.Service;
namespace EBS.Application.Facade
{
   public class VipProductFacade:IVipProductFacade
    {
        IDBContext _db;
        VipProductService _vipProductService;
     
        public VipProductFacade(IDBContext dbContext)
        {
            _db = dbContext;
            _vipProductService = new VipProductService(this._db);
        }
       public void Create(string vipProducts)
        {
            var products= JsonConvert.DeserializeObject<List<VipProduct>>(vipProducts);
            _vipProductService.Create(products);
            _db.SaveChange();
        }

        public void Edit(VipProductModel model)
        {
            throw new NotImplementedException();
        }
    }
}
