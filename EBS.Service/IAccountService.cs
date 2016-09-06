using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Command.Models;
namespace EBS.Command
{
    public interface IAccountService
    {
        AccountInfo Login(LoginModel model);

    }
}
