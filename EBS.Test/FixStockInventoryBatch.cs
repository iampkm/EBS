using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EBS.Infrastructure;
using Dapper.DBContext;
using EBS.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
namespace EBS.Test
{
    /// <summary>
    /// 这不是单元测试，这是利用单元测试功能来修复批次库存数据的代码
    /// </summary>
    [TestClass]
    public class FixStockInventoryBatch
    {
        [TestMethod]
        public void FixBatchInventory()
        {
            IDBContext db = new DapperDBContext("masterDB");

            // 查询 异常批次数据
            var sql = @"select p.id,p.`Code`,p.BarCode,s.StoreId,s.Quantity,t.Quantity as bqty,c.Quantity as hqty from storeinventory s left JOIN
(select b.StoreId,b.ProductId,sum(b.Quantity) as Quantity from storeinventorybatch b group by b.StoreId,b.ProductId) t
on s.StoreId = t.StoreId and s.ProductId = t.ProductId
left join 
(select h.StoreId,h.ProductId,sum( h.ChangeQuantity) as Quantity from storeinventoryhistory h GROUP BY h.StoreId,h.ProductId ) c
on  s.StoreId = c.StoreId and s.ProductId = c.ProductId
left join product p on p.id = s.ProductId
where s.Quantity<>c.Quantity or c.Quantity<>t.Quantity or s.Quantity<>t.Quantity ";
            var waitFixProducts = db.Table.FindAll<FixProduct>(sql, null);
            foreach (var item in waitFixProducts)
            {
                if (item.Quantity <= 0)
                {
                    //更新当前所有商品批次库存为0
                    if (item.Bqty > 0)
                    {
                        string sqlUpdate = "update storeinventorybatch set Quantity = 0 where StoreId=@StoreId and ProductId=@ProductId and Quantity>0";
                        db.Command.AddExecute(sqlUpdate, new { StoreId = item.StoreId, ProductId = item.Id });
                    }                   
                }
                else
                {
                    // 把批次库存数修复成与总库存一致
                    var inventoryBatchs = db.Table.FindAll<StoreInventoryBatch>("select * from storeinventorybatch where  storeId=@StoreId and productId = @ProductIds and Quantity>0", new { StoreId = item.StoreId, ProductIds = item.Id }).OrderBy(n=>n.BatchNo).ToList();
                    if (item.Quantity < item.Bqty)
                    {
                        var updateList = MinusInventory(inventoryBatchs, item.Bqty - item.Quantity);
                        if (updateList.Count > 0) {
                            db.Update(updateList.ToArray());
                        }
                    }
                    else {
                       
                    }
                }

            }

          
          // db.SaveChange();
           Assert.AreEqual(1, 1);

        }

        public List<StoreInventoryBatch> MinusInventory(List<StoreInventoryBatch> list, int quantity)
        {
            var leftQty = quantity;
            var result = new List<StoreInventoryBatch>();
            foreach (var item in list)
            {
                if (item.Quantity >= leftQty)
                {
                    item.Quantity = item.Quantity - leftQty;
                    result.Add(item);
                    break;
                }
                else {
                    leftQty = leftQty - item.Quantity;
                    item.Quantity = 0;
                    result.Add(item);
                }
            }
            return result;
        }
    }

    public class FixProduct
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string BarCode { get; set; }

        public int StoreId { get; set; }
        /// <summary>
        /// 总库存
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 批次总库存
        /// </summary>
        public int Bqty { get; set; }
        /// <summary>
        /// 历史库存合计数 
        /// </summary>
        public int Hqty { get; set; }
    }
}
