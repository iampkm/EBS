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
using EBS.Application;
using EBS.Application.Facade;
using EBS.Infrastructure.Queue;
using PaySharp.Alipay;
using PaySharp.Core;
//using PaySharp.Core.Mvc;
using PaySharp.Wechatpay;
using EBS.Domain.Entity;
using EBS.Domain;
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
            // register database connection
            builder.RegisterType<DapperDBContext>().As<IDBContext>().WithParameter("connectionStringName", "masterDB");
            builder.RegisterType<QueryService>().As<IQuery>().WithParameter("connectionStringName", "masterDB");

            //注册销售单队列处理，单例
            builder.RegisterType<PosSyncFacade>().As<IQueueHander<string>>();
            builder.RegisterType<SimpleQueue<string>>().As<ISimpleQueue<string>>().SingleInstance();
            // builder.RegisterInstance(new DapperDBContext(Configer.MasterDB)).As<IDBContext>().InstancePerLifetimeScope();
            // builder.RegisterType<DapperDBContext>().WithParameter(Configer.MasterDB).As<IDBContext>().SingleInstance();
            // builder.RegisterAssemblyTypes(webAssembly);

            // register validator

            //builder.RegisterType<NewsCategoryValidator>().As<IValidator<NewsCategoryModel>>();
            //builder.RegisterType<NewsValidator>().As<IValidator<NewsModel>>();            
            builder.Update(AppContext.Container);

            // 注册支付组件
           // RegisterPaySetting();


            //ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AutofacValidatorFactory(AppContext.Container)));
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(AppContext.Container));
            // start auto task
            //if (Configer.OpenTask)
            //{
            //    ScheduleContext.TaskConfigPath = HttpRuntime.AppDomainAppPath + "Task.Config";
            //    ScheduleContext.Start();
            //}

            // setting validator local
            // 设置 FluentValidation 默认的资源文件提供程序 - 中文资源
            ValidatorOptions.ResourceProviderType = typeof(FluentValidationResource);

            /* 比如验证用户名 not null、not empty、length(2,int.MaxValue) 时，链式验证时，如果第一个验证失败，则停止验证 */
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure; // ValidatorOptions.CascadeMode 默认值为：CascadeMode.Continue


            // 配置 FluentValidation 模型验证为默认的 ASP.NET MVC 模型验证
            // FluentValidationModelValidatorProvider.Configure();
        }

        /// <summary>
        ///  组合成门店key
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private static string StoreKey(string keyName,int storeId)
        {
            return keyName + "." + storeId.ToString();
        }

        public static void RegisterPaySetting()
        {
            var builder = new ContainerBuilder();
            var gateways = new Gateways();
         
            var db = AppContext.Current.Resolve<IDBContext>();

            // 注册所有配置
            var allSettings = db.Table.FindAll<Setting>().ToList();  // 所有支付参数
            builder.RegisterInstance(new SettingsCollection(allSettings)).As<Domain.ISettings>().SingleInstance();

            // 注册支付配置
            var settings = allSettings.Where(n=>n.KeyName.StartsWith("pay.")).ToList(); 
            var normalSettings = settings.ToDictionary(n => StoreKey(n.KeyName,n.StoreId)); // key+ 门店ID 唯一
            //按门店设置商户号参数
            var storeSettings = settings.GroupBy(n => n.StoreId);
            var domainUrl = allSettings.FirstOrDefault(n => n.KeyName == SettingKeys.System_Domain).Value;
            foreach (var storeSettingGroup in storeSettings)
            {
                /// 支付宝配置
                var alipaySetting = storeSettingGroup.Where(n => n.KeyName.StartsWith("pay.alipay")).ToList().ToDictionary(n => StoreKey(n.KeyName,n.StoreId));
                var storeid = storeSettingGroup.Key;
                var alipayMerchant = new PaySharp.Alipay.Merchant
                {
                    AppId = alipaySetting.ContainsKey(StoreKey(SettingKeys.Pay_Alipay_Appid,storeid)) ? alipaySetting[StoreKey(SettingKeys.Pay_Alipay_Appid, storeid)].Value : "",
                    NotifyUrl = domainUrl+ (normalSettings.ContainsKey(StoreKey(SettingKeys.Pay_Notify_Url, storeid)) ? normalSettings[StoreKey(SettingKeys.Pay_Notify_Url, storeid)].Value : ""),
                    ReturnUrl = domainUrl+ (normalSettings.ContainsKey(StoreKey(SettingKeys.Pay_Return_Url, storeid)) ? normalSettings[StoreKey(SettingKeys.Pay_Return_Url, storeid)].Value : ""),
                    AlipayPublicKey = alipaySetting.ContainsKey(StoreKey(SettingKeys.Pay_Alipay_Public_Key, storeid)) ? alipaySetting[StoreKey(SettingKeys.Pay_Alipay_Public_Key, storeid)].Value : "",
                    Privatekey = alipaySetting.ContainsKey(StoreKey(SettingKeys.Pay_Alipay_Private_Key, storeid)) ? alipaySetting[StoreKey(SettingKeys.Pay_Alipay_Private_Key, storeid)].Value : "",
                    StoreId = storeid
                };
                gateways.Add(new AlipayGateway(alipayMerchant)
                {
                    GatewayUrl = "https://openapi.alipaydev.com"
                });

                //微信配置
                var wechatSetting = storeSettingGroup.Where(n => n.KeyName.StartsWith("pay.wechat")).ToList().ToDictionary(n => StoreKey(n.KeyName, n.StoreId));
                var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                {
                    AppId = wechatSetting.ContainsKey(StoreKey(SettingKeys.Pay_Wechat_Appid, storeid)) ? wechatSetting[StoreKey(SettingKeys.Pay_Wechat_Appid, storeid)].Value : "",
                    MchId = wechatSetting.ContainsKey(StoreKey(SettingKeys.Pay_Wechat_MchId, storeid)) ? wechatSetting[StoreKey(SettingKeys.Pay_Wechat_MchId, storeid)].Value : "",
                    Key = wechatSetting.ContainsKey(StoreKey(SettingKeys.Pay_Wechat_MchKey, storeid)) ? wechatSetting[StoreKey(SettingKeys.Pay_Wechat_MchKey, storeid)].Value : "",
                    AppSecret = wechatSetting.ContainsKey(StoreKey(SettingKeys.Pay_Wechat_AppSecret, storeid)) ? wechatSetting[StoreKey(SettingKeys.Pay_Wechat_AppSecret, storeid)].Value : "",
                    //SslCertPath = AppDomain.CurrentDomain.BaseDirectory + "Certs/apiclient_cert.p12",
                    //SslCertPassword = "1233410002",
                    NotifyUrl = domainUrl + (normalSettings.ContainsKey(StoreKey(SettingKeys.Pay_Notify_Url, storeid)) ? normalSettings[StoreKey(SettingKeys.Pay_Notify_Url, storeid)].Value : ""),
                    StoreId = storeid
                };
                gateways.Add(new WechatpayGateway(wechatpayMerchant));
            }

           // builder.RegisterInstance(new DapperDBContext(Configer.MasterDB)).As<IDBContext>().InstancePerLifetimeScope();
            builder.RegisterInstance(gateways).As<IGateways>().SingleInstance();

            // 注册支付路由
            PayServices.IPayRoute route = new PayServices.DefaultPayRoute();
            route.InitRoute();

            builder.RegisterInstance(route).As<PayServices.IPayRoute>().SingleInstance();

            builder.Update(AppContext.Container);
        }

    }
}