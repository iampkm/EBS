using EBS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface IStocktakingPlanFacade
    {
       void Create(StocktakingPlanModel model);

       void Edit(StocktakingPlanModel model);
    }

}
