﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EBS.Domain.Accounts
{
   public interface IAccountLogic
    {
       bool VerifyAccount(string userName, string password);
      
    }
}
