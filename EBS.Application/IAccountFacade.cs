using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Application.DTO;
namespace EBS.Application
{
    public interface IAccountFacade
    {
        AccountInfo Login(LoginModel model);

        void Create(CreateAccountModel model);

        void Edit(EditAccountModel model);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPassword"></param>
        void ChangePassword(int id, string oldPassword, string newPassword);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        void ResetPassword(int id);
        /// <summary>
        /// 激活账户
        /// </summary>
        /// <param name="id"></param>
        void ActiveAccount(int id);
        /// <summary>
        /// 禁用账户
        /// </summary>
        /// <param name="id"></param>
        void DisabledAccount(int id);
       

    }
}
