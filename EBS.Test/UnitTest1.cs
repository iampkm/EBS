using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EBS.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var today = DateTime.Now;
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1); // 统计当月所有数据

            Assert.AreEqual("2017-03-01",endDate.ToString("yyyy-MM-dd"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var actual= CalculatedAveragePrice(0,0,2.3456m,10);
            Assert.AreEqual(2.3456m,actual);

             actual = CalculatedAveragePrice(3.56m, 10, 2.3456m, 10);
            Assert.AreEqual(2.9528m, actual);

             actual = CalculatedAveragePrice(3.56m, -100, 2.3456m, 10);
             Assert.AreEqual(3.6949m, actual);
            actual = CalculatedAveragePrice(3.56m, -100, 2.3456m, 100);
            Assert.AreEqual(2.3456m, actual);

            actual = CalculatedAveragePrice(3.56m, -100, 2.3456m, -100);
            Assert.AreEqual(2.9528m, actual);
        }

        private decimal CalculatedAveragePrice(decimal CurrentAvgCostPrice, int CurrentQuantity, decimal price, int quantity)
        {
            // 修改库存均价
            int totalQuantity = CurrentQuantity + quantity;
            var avgCostPrice = totalQuantity == 0 ? price : Math.Round((CurrentAvgCostPrice * CurrentQuantity + price * quantity) / totalQuantity, 4);
            return avgCostPrice;
        }
    }
}
