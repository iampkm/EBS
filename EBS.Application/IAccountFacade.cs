using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.Models;
namespace EBS.Application
{
    public interface IAccountFacade
    {
        AccountInfo Login(LoginModel model);

    }
}
