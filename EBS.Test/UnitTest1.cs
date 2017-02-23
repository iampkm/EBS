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
    }
}
