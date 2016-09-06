using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace EBS.Infrastructure
{
    public class Configer
    {
        public static string MasterDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["masterDB"].ConnectionString;
            }
        }
        public static bool OpenTask
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["OpenTask"]);
            }
        }
        /// <summary>
        /// 业务组件
        /// </summary>
        public static string[] BusinessAssemblies
        {
            get
            {
                return new string[] { "EBS.Command", "EBS.Command.Service", "EBS.Query", "EBS.Query.Service", "EBS.Domain", };
            }
        }
    }
}
