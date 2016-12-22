using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface ISupplierFacade
    {
       void Create(SupplierModel model);
       void Edit(SupplierModel model);
        void Delete(string ids);
        void ImportProduct(string supplierProductJson, int updatedBy);

        void removeProduct(string ids);
        /// <summary>
        ///  标记待候选
        /// </summary>
        /// <param name="id"></param>
        void MarkWaitSuppply(int markId,int unMarkId, int updatedBy);

        void UnMarkWaitSuppply(int markId, int unMarkId, int updatedBy);

    }
}
