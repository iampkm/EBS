﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using EBS.Application.DTO;
namespace EBS.Application
{
   public interface IStoreFacade
    {
        void Create(StoreModel model);
        void Edit(StoreModel model);
        void Delete(string ids);
    }
}
