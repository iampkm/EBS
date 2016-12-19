using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Domain.ValueObject;
using Dapper.DBContext;
namespace EBS.Domain.Service
{
   public class BillSequenceService
    {
        IDBContext _db;
        public BillSequenceService(IDBContext dbContext)
        {
            this._db = dbContext;
        }
       /// <summary>
       /// 单据号生成算法
       /// </summary>
       /// <param name="billIdentity"></param>
       /// <returns></returns>
       public string GenerateNewCode(BillIdentity billIdentity)
       {
           // 生成一个新的 Code 序列号 
           var billId = ((int)billIdentity).ToString();
           billId = billId.Substring(0, 2);
           var codeSequence = new BillSequence();
           _db.Insert<BillSequence>(codeSequence);
           _db.SaveChange();
           codeSequence = _db.Table.Find<BillSequence>(n => n.GuidCode == codeSequence.GuidCode);
           var sequenceId = codeSequence.Id > 99999999 ? codeSequence.Id.ToString() : codeSequence.Id.ToString().PadLeft(8, '0');
           return billId + sequenceId;
       }

       /// <summary>
       /// 入库批次号算法:年月日时分秒+两位随机数
       /// </summary>
       /// <returns></returns>
       public string GenerateBatchNo()
       {
            // 每天，门店的采购单顺序号：  日期+01
           var date = DateTime.Now;
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            var rdNumber = rd.Next(0, 100).ToString();
           return string.Format("{0}{1}", date.ToString("yyyyMMddHHmmss"), rdNumber);
       }

        //public string GenerateBatchNo(int storeId)
        //{
        //    _db.Table.Context.ExecuteScalar<int>("select count(*) from StorePurchaseOrder where CreatedOn>=")
        //}
    }
}
