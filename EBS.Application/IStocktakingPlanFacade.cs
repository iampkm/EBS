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

       void StartPlan(int id, int editedBy, string editor);

       void MergeDetial(int id, int editedBy, string editor);

       void EndPlan(int id, int editedBy, string editor, string loginPassword);
    }

}
