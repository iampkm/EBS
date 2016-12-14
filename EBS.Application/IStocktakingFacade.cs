using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IStocktakingFacade
    {
       /// <summary>
       /// 创建盘点单
       /// </summary>
       /// <param name="model"></param>
       void Create(StocktakingModel model);
       /// <summary>
       /// 创建盘点修正单
       /// </summary>
       /// <param name="model"></param>
       void Correct(StocktakingModel model);

       void Edit(StocktakingModel model);

       void Audit(int id);


    }
}
