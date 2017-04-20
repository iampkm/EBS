using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Application
{
   public interface IPurchaseSaleInventoryFacade
    {
        /// <summary>
        /// 生成进销存报表
        /// </summary>
        /// <param name="today"></param>
        void Generate(DateTime today);
        /// <summary>
        /// 生成进销存明细报表
        /// </summary>
        /// <param name="today"></param>
        void GenerateDetail(DateTime today);
    }
}
