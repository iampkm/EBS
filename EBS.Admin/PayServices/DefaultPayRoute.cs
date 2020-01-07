using PaySharp.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBS.Admin.PayServices
{
    public class DefaultPayRoute : IPayRoute
    {
        private readonly static Dictionary<string, PayRouteMapper> _routes = new Dictionary<string, PayRouteMapper>();

        
        public PayRouteMapper Find(string method)
        {
            if (string.IsNullOrEmpty(method)) {
                throw new Exception("接口参数 method 不能为空");
            }
            if (!_routes.ContainsKey(method.ToLower())) {
                throw new Exception(string.Format("请求接口 method={0} 不存在",method));
            }
            return _routes[method];
        }

        public void InitRoute()
        {
            var interfaceType = typeof(IPayAgent);
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("CBest")).ToList();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var mapperTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => !t.IsGenericType && t.IsClass && interfaceType.IsAssignableFrom(t))).ToList();
            // var mappers = mapperTypes.Select(t => Activator.CreateInstance(t) as IAutoMapperRegistrar);

            foreach (var agentType in mapperTypes)
            {              
                var methodInofs = agentType.GetMethods();
                foreach (var mi in methodInofs)
                {                   
                    var attrs=mi.GetCustomAttributes(typeof(PayRouteAttribute),true);
                    if (attrs != null && attrs.Length > 0 && !_routes.ContainsKey((attrs[0] as PayRouteAttribute).RouteName)) {
                        var routeData = new PayRouteMapper();
                        routeData.AgentMethodName = mi.Name;
                        routeData.AgentName = agentType.Name;
                        var attr = attrs[0] as PayRouteAttribute;
                        routeData.RouteName = attr.RouteName;
                        routeData.AgentType = agentType;
                        routeData.AgentMethod = mi;
                        _routes.Add(routeData.RouteName, routeData);
                    }
                }
            }

        }
    }
}