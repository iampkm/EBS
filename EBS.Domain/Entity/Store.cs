﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain.Entity
{
    /// <summary>
    /// 门店
    /// </summary>
   public class Store:BaseEntity
    {
       public Store() {
           this.SourceKey = Guid.NewGuid().ToString().Replace("-", "");
       }
       public string Name { get; set; }
       /// <summary>
       /// 门店编码:区域 2位 + 4位 顺序码  10_0001
       /// </summary>
       public string Code { get; set; }
       /// <summary>
       /// 代码数字
       /// </summary>
       public int Number { get; set; }
        public string SourceKey { get; set; }
        /// <summary>
        /// 6位区域编码
        /// </summary>
        public string AreaId { get; set; }
        public string Address { get; set; }       
        public string Contact { get; set; }       
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string Setting { get; set; }

        public void GenerateNewCode(int maxNumber)
        {
            var firstAreaId = this.AreaId.Substring(0, 2);
            var nextAreaIdNumber = maxNumber + 1;
            this.Code = string.Format("{0}{1}", firstAreaId, nextAreaIdNumber.ToString().PadLeft(4, '0'));
            this.Number = nextAreaIdNumber;
        }
    }
}
