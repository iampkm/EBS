using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EBS.Admin.PayServices
{
    public class PayRouteMapper
    {

        public string RouteName { get; set; }

        /// <summary>
        ///  支付代理类名
        /// </summary>
        public string AgentName { get; set; }
        /// <summary>
        ///  支付代理方法名
        /// </summary>

        public string AgentMethodName { get; set; }

        public Type AgentType { get; set; }

        public MethodInfo AgentMethod { get; set; }
    }
}