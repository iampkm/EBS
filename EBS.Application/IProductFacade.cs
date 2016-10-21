using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IProductFacade
    {
        void Create(ProductModel model);
        void Edit(ProductModel model);
    }
}
