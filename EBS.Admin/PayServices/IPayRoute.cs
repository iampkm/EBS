using PaySharp.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBS.Admin.PayServices
{
    public interface IPayRoute
    {
        void InitRoute();

        PayRouteMapper Find(string method);
      
    }
}