using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface ISaleReportFacade
    {
       void Create(DateTime beginDate, DateTime endDate);
    }
}
