using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using EBS.Infrastructure;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using EBS.Infrastructure.Task;
using EBS.Infrastructure.Log;
using EBS.Admin.Services;
using Dapper.DBContext;
using FluentValidation;
namespace EBS.Admin
{
    public class AppConfig
    {
        public static void Start()
        {
            AppContext.Init();

            var builder = new ContainerBuilder();
            //  ASP.NET MVC Autofac RegisterDependency     
            Assembly webAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(webAssembly);
            // register admin service
           builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
           builder.RegisterType<ContextService>().As<IContextService>().InstancePerLifetimeScope();
           builder.RegisterType<DapperDBContext>().As<IDBContext>().WithParameter("connectionStringName", "masterDB");                                                                                   
          // builder.RegisterInstance(new DapperDBContext(Configer.MasterDB)).As<IDBContext>().InstancePerLifetimeScope();
          // builder.RegisterType<DapperDBContext>().WithParameter(Configer.MasterDB).As<IDBContext>().SingleInstance();
           // builder.RegisterAssemblyTypes(webAssembly);

            // register validator
          
           //builder.RegisterType<NewsCategoryValidator>().As<IValidator<NewsCategoryModel>>();
           //builder.RegisterType<NewsValidator>().As<IValidator<NewsModel>>();          

            builder.Update(AppContext.Container);
            //ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AutofacValidatorFactory(AppContext.Container)));
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(AppContext.Container));
            // start auto task
            if (Configer.OpenTask)
            {
                ScheduleContext.TaskConfigPath = HttpRuntime.AppDomainAppPath + "Task.Config";
                ScheduleContext.Start();
            }

            // setting validator local
            // 设置 FluentValidation 默认的资源文件提供程序 - 中文资源
            ValidatorOptions.ResourceProviderType = typeof(FluentValidationResource);

            /* 比如验证用户名 not null、not empty、length(2,int.MaxValue) 时，链式验证时，如果第一个验证失败，则停止验证 */
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure; // ValidatorOptions.CascadeMode 默认值为：CascadeMode.Continue


            // 配置 FluentValidation 模型验证为默认的 ASP.NET MVC 模型验证
           // FluentValidationModelValidatorProvider.Configure();
        }
       
    }
}