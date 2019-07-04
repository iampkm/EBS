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
            RegisterPaySetting();


            //PaySharpConfig.Register(typeof(MvcApplication), new ContainerBuilder(), a =>
            //{
            //    var gateways = new Gateways();
            //    //gateways.RegisterAlipay();
            //    var db = AppContext.Current.Resolve<IDBContext>();
            //    var settings = db.Table.FindAll<Setting>(n => n.KeyType.Like("pay.%")).ToList();  // 所有支付参数
            //    var normalSettings = settings.ToDictionary(n => n.KeyName);
            //    var storeSettings = settings.GroupBy(n => n.StoreId);

            //    foreach (var storeSettingGroup in storeSettings)
            //    {

            //    //var alipayMerchant = new PaySharp.Alipay.Merchant
            //    //{
            //    //    AppId = "2016081600256163",
            //    //    NotifyUrl = "http://localhost:61378/Notify",
            //    //    ReturnUrl = "http://localhost:61378/Notify",
            //    //    AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsW6+mN2E3Oji2DPjSKuYgRzK6MlH9q6W0iM0Yk3R0qbpp5wSesSXqudr2K25gIBOTCchiIbXO7GXt/zEdnhnC32eOaTnonDsnuBWIp+q7LoVx/gvKIX5LTHistCvGli8VW4EDGsu2jAyQXyMPgPrIz+/NzWis/gZsa4TaqVY4SpWRuSgMXxleh2ERB6k0ijK0IYM+Cv5fz1ZPDCgk7EbII2jk2fDxtlMLoN5UYEJCcD8OUyivm3Hti3u1kPolckCCf0xk+80g/4EdmzFAffsVgPeXZrkm5EIuiTTOIeRHXlTg3HtkkCw2Wl0CpYSKBr9Vzv7x0gNvb1wnXPmBJNRgQIDAQAB",
            //    //    Privatekey = "MIIEpAIBAAKCAQEAyC43UbsE5XZ2Pmqg1YgzeCqAMk4HOH8fYHslseeSgKxyDjybjqM0yjGIJry1FRmVvLnY7v8jURgwr7d/pDCSRdoHa6zaxuSzg0OlieNmujae34YZ54PmFxULZW0BHSdzmx3OIYK2GarRECkds531ZzpbLdRXqsxQf5G26JZLIFxmNuh/VjBjJ6Hic1WOFT+FCYyi8om+LkPn3jELeA7LPLXzFqzzxx0vo4yiAePrsX5WucWxf+Y8rZoDhRIy/cPtQECXi9SiAWOJe/82JqjVjfpowf3QN7UJHsA82RBloAS4lvvDGJA7a+8DDlqpqPer8cS41Dv5r39iqtJUybDqoQIDAQABAoIBAHi39kBhiihe8hvd7bQX+QIEj17G02/sqZ1jZm4M+rqCRB31ytGP9qvghvzlXEanMTeo0/v8/O1Qqzusa1s2t19MhqEWkrDTBraoOtIWwsKVYeXmVwTY9A8Db+XwgHV2by8iIEbxLqP38S/Pu8uv/GgONyJCJcQohnsIAsfsqs2OGggz+PplZaXJfUkPomWkRdHM9ZWWDLrCIlmRSHLmhHEtFJaXD083kqo437qra58Amw/n+2gH57utbAQ9V3YQFjD8zW511prC+mB6N/WUlaLstkxswGJ16obEJfQ0r8wYHx14ep6UKGyi3YXlMHcteI8gz+uFx4RuVV9EotdXagECgYEA7AEz9oPFYlW1H15OkDGy8yBnpJwIBu2CQLxINsxhrLIAZ2Bgxqcsv+D9CpnYCBDisbXoGoyMK6XaSypBMRKe2y8yRv4c+w00rcKHtGfRjzSJ5NQO0Tv+q8vKY+cd6BuJ6OUQw82ICLANIfHJZNxtvtTCmmqBwSJDpcQJQXmKXTECgYEA2SQCSBWZZONkvhdJ15K+4IHP2HRbYWi+C1OvKzUiK5bdJm77zia4yJEJo5Y/sY3mV3OK0Bgb7IAaxL3i0oH+WNTwbNoGpMlYHKuj4x1453ITyjOwPNj6g27FG1YSIDzhB6ZC4dBlkehi/7gIlIiQt1wkIZ+ltOqgI5IqIdXoSHECgYB3zCiHYt4oC1+UW7e/hCrVNUbHDRkaAygSGkEB5/9QvU5tK0QUsrmJcPihj/RUK9YW5UK7b0qbwWWsr/dFpLEUi8GWvdkSKuLprQxbrDN44O96Q5Z96Vld9WV4DtJkhs4bdWNsMQFzf4I7D9PuKeJfcvqRjaztz6nNFFSqcrqkkQKBgQCJKlUCohpG/9notp9fvQQ0n+viyQXcj6TVVOSnf6X5MRC8MYmBHTbHA8+59bSAfanO/l7muwQQro+6TlUVMyaviLvjlwpxV/sACXC6jCiO06IqreIbXdlJ41RBw2op0Ss5gM5pBRLUS58V+HP7GBWKrnrofofXtAq6zZ8txok4EQKBgQCXrTeGMs7ECfehLz64qZtPkiQbNwupg938Z40Qru/G1GR9u0kmN7ibTyYauI6NNVHGEZa373EBEkacfN+kkkLQMs1tj5Zrlw+iITm+ad/irpXQZS/NHCcrg6h82vu0LcgiKnHKlmW6K5ne0w4LqmsmRCm7JdJjt9WlapAs0ticiw=="
            //    //};

            //    //var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
            //    //{
            //    //    AppId = "wx2428e34e0e7dc6ef",
            //    //    MchId = "1233410002",
            //    //    Key = "e10adc3849ba56abbe56e056f20f883e",
            //    //    AppSecret = "51c56b886b5be869567dd389b3e5d1d6",
            //    //    //SslCertPath = AppDomain.CurrentDomain.BaseDirectory + "Certs/apiclient_cert.p12",
            //    //    //SslCertPassword = "1233410002",
            //    //    NotifyUrl = "http://localhost:61378/Notify"
            //    //};
            //    //gateways.Add(new AlipayGateway(alipayMerchant)
            //    //{
            //    //    GatewayUrl = "https://openapi.alipaydev.com"
            //    //});
            //    //gateways.Add(new WechatpayGateway(wechatpayMerchant));
            //    //gateways.Add(new UnionpayGateway(unionpayMerchant)
            //    //{
            //    //    GatewayUrl = "https://gateway.test.95516.com"
            //    //});



            //    return gateways;
            //});

            //  builder.Update(AppContext.Container);

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

        public static void RegisterPaySetting()
        {
            var builder = new ContainerBuilder();
            var gateways = new Gateways();
            //gateways.RegisterAlipay();
            var db = AppContext.Current.Resolve<IDBContext>();
            var settings = db.Table.FindAll<Setting>(n => n.KeyType.Like("pay.%")).ToList();  // 所有支付参数
            var normalSettings = settings.ToDictionary(n => n.KeyName);
            var storeSettings = settings.GroupBy(n => n.StoreId);

            foreach (var storeSettingGroup in storeSettings)
            {
                /// 支付宝配置
                var alipaySetting = storeSettingGroup.Where(n => n.KeyType.StartsWith("pay.aliypay")).ToList().ToDictionary(n => n.KeyName);
                var alipayMerchant = new PaySharp.Alipay.Merchant
                {
                    AppId = alipaySetting.ContainsKey("pay.alipay.appid") ? alipaySetting["pay.alipay.appid"].Value : "",
                    NotifyUrl = normalSettings.ContainsKey("pay.notify.url") ? alipaySetting["pay.alipay.notify.url"].Value : "",
                    ReturnUrl = normalSettings.ContainsKey("pay.return.url") ? alipaySetting["pay.alipay.notify.url"].Value : "",
                    AlipayPublicKey = alipaySetting.ContainsKey("pay.alipay.publickey") ? alipaySetting["pay.alipay.publickey"].Value : "",
                    Privatekey = alipaySetting.ContainsKey("pay.alipay.privatekey") ? alipaySetting["pay.alipay.publickey"].Value : "",
                };
                gateways.Add(new AlipayGateway(alipayMerchant)
                {
                    GatewayUrl = "https://openapi.alipaydev.com"
                });

                //微信配置
                var wechatSetting = storeSettingGroup.Where(n => n.KeyType.StartsWith("pay.aliypay")).ToList().ToDictionary(n => n.KeyName);
                var wechatpayMerchant = new PaySharp.Wechatpay.Merchant
                {
                    AppId = alipaySetting.ContainsKey("pay.wechat.appid") ? alipaySetting["pay.wechat.appid"].Value : "",
                    MchId = alipaySetting.ContainsKey("pay.wechat.mchid") ? alipaySetting["pay.wechat.mchid"].Value : "",
                    Key = alipaySetting.ContainsKey("pay.wechat.mchkey") ? alipaySetting["pay.wechat.mchkey"].Value : "",
                    AppSecret = alipaySetting.ContainsKey("pay.wechat.appsecret") ? alipaySetting["pay.wechat.appsecret"].Value : "",
                    //SslCertPath = AppDomain.CurrentDomain.BaseDirectory + "Certs/apiclient_cert.p12",
                    //SslCertPassword = "1233410002",
                    NotifyUrl = normalSettings.ContainsKey("pay.notify.url") ? alipaySetting["pay.notify.url"].Value : "",
                };
                gateways.Add(new WechatpayGateway(wechatpayMerchant));
            }

           // builder.RegisterInstance(new DapperDBContext(Configer.MasterDB)).As<IDBContext>().InstancePerLifetimeScope();
            builder.RegisterInstance(gateways).As<IGateways>().SingleInstance();
            builder.Update(AppContext.Container);
        }

    }
}