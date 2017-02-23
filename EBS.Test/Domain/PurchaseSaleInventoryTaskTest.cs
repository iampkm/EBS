using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EBS.Domain.Service;
using EBS.Infrastructure;
using Dapper.DBContext;
using System.Reflection;
using Autofac;
namespace EBS.Test.Domain
{
    [TestClass]
    public class PurchaseSaleInventoryTaskTest
    {
        [TestMethod]
        public void Test_PurchaseSaleInventory()
        {
            AppContext.Init();
            var builder = new ContainerBuilder();
            ////  ASP.NET MVC Autofac RegisterDependency     
            //Assembly webAssembly = Assembly.GetExecutingAssembly();
            //builder.RegisterControllers(webAssembly);

            builder.RegisterType<DapperDBContext>().As<IDBContext>().WithParameter("connectionStringName", "masterDB");
            builder.RegisterType<QueryService>().As<IQuery>().WithParameter("connectionStringName", "masterDB");
            builder.Update(AppContext.Container);
            PurchaseSaleInventoryTask task = new PurchaseSaleInventoryTask();
            task.Execute();
            Assert.AreEqual(1, 1);
        }
    }
}
