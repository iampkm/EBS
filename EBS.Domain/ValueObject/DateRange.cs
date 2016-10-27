using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.ValueObject
{
   public class DateRange
    {
       public DateRange() { }
       public DateRange(DateTime from, DateTime to)
       {
           this.StartDate = from;
           this.EndDate = to;
       }
       public DateTime StartDate { get;  set; }
       public DateTime EndDate { get;  set; }
    }
}
