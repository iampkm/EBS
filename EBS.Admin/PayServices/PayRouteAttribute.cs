using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBS.Admin.PayServices
{
    public class PayRouteAttribute:Attribute
    {

        public PayRouteAttribute(string routeName)
        {
            this.RouteName = routeName;
        }

        public string RouteName { get; set; }
    }
}