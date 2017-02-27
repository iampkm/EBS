using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure;
using EBS.Infrastructure.Task;
using Dapper.DBContext;
using System.Reflection;
using Autofac;
using EBS.Infrastructure.Log;
namespace EBS.AutoTask
{
    public partial class TaskService : ServiceBase
    {
        ILogger _log;
        public TaskService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppContext.Init();
            var builder = new ContainerBuilder();
            builder.RegisterType<DapperDBContext>().As<IDBContext>().WithParameter("connectionStringName", "masterDB");
            builder.RegisterType<QueryService>().As<IQuery>().WithParameter("connectionStringName", "masterDB");
            builder.Update(AppContext.Container);

            _log = AppContext.Current.Resolve<ILogger>();
            ScheduleContext.TaskConfigPath = AppDomain.CurrentDomain.BaseDirectory + "\\Task.Config";
            ScheduleContext.Start();

            _log.Info("ebs 自动任务已启动");
        }

        protected override void OnStop()
        {
            ScheduleContext.Close();
             _log.Info("ebs 自动任务停止");
        }
    }
}
