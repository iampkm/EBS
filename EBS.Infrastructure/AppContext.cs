using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Infrastructure.IoC;
using Autofac;
using EBS.Infrastructure.Log;
using System.Reflection;
using System.Web;
using EBS.Infrastructure.Events;
using EBS.Infrastructure.Caching;
namespace EBS.Infrastructure
{
    public abstract class AppContext
    {
        static IContainer _autofacContainer;
        public static void Init()
        {
            var builder = new ContainerBuilder();
            // builder.RegisterInstance(LogFactory.CreateLoger("log4Net")).As<ILog>();
            //注册程序集
            //  builder.RegisterType<Log4NetWritter>().As<ILogger>();
            builder.RegisterInstance(NLog.LogManager.GetCurrentClassLogger()).As<NLog.ILogger>();
            builder.RegisterType<NLogWriter>().As<ILogger>();
            // register dependency
            var typeFinder = new DefaultTypeFinder();
            // builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();           
            // var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            // var drInstances = new List<IDependencyRegistrar>();
            // foreach (var drType in drTypes)
            //     drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //// drInstances = drInstances.AsQueryable().ToList();
            // foreach (var dependencyRegistrar in drInstances)
            // {
            //     dependencyRegistrar.Register(builder, typeFinder);
            // } 

            //注册各层业务组件
            var assemblies = typeFinder.LoadAssemblies();
            //builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Logic")).AsImplementedInterfaces();

            //register event
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            // register validator
            //var validators = typeFinder.FindClassesOfType(typeof(IValidator<>)).ToList();
            //foreach (var validator in validators)
            //{
            //    builder.RegisterType(validator)
            //        .As(validator.FindInterfaces((type, criteria) =>
            //        {
            //            var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
            //            return isMatch;
            //        }, typeof(IValidator<>)))
            //        .InstancePerLifetimeScope();
            //}

            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();

            _autofacContainer = builder.Build();
            // 初始化容器管理对象
            Singleton<IContainerManager>.Instance = new AutofacContainer(_autofacContainer);

        }

        public static IContainer Container
        {
            get { return _autofacContainer; }
        }

        public static IContainerManager Current
        {
            get
            {
                if (Singleton<IContainerManager>.Instance == null)
                {
                    Init();
                }
                return Singleton<IContainerManager>.Instance;
            }
        }
    }
}
