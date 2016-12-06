using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
namespace EBS.Application.DTO
{
   public class LoginModel
    {
       public string UserName { get; set; }

       public string Password { get; set; }

       public bool RememberMe{ get; set; }
       public string IpAddress { get; set; }

       public string ReturnUrl { get; set; }

       public void Validate()
       {
           LoginModelValidator validator = new LoginModelValidator();
           ValidationResult result = validator.Validate(this);
           if (!result.IsValid)
           {
               StringBuilder sb = new StringBuilder();
               result.Errors.ToList().ForEach(error => sb.Append(error.ErrorMessage+";"));
               throw new Exception(sb.ToString());
           } 
       }
    }

   public class LoginModelValidator: AbstractValidator<LoginModel>
   {
       //.WithLocalizedMessage(() => "请输入{PropertyName}")
       public LoginModelValidator() {
           RuleFor(m => m.UserName).NotEmpty().WithLocalizedName(() => "账户").WithLocalizedMessage(() => "请输入{PropertyName}");
           RuleFor(m => m.Password).NotEmpty().WithLocalizedName(() => "密码").WithLocalizedMessage(() => "请输入{PropertyName}");
       }
   }

   public class AccountInfo
   {
       public int AccountId { get; set; }
       public string UserName { get; set; }
       public string NickName { get; set; }
       public int RoleId { get; set; }

       public int StoreId { get; set; }
        /// <summary>
        /// 逗号分隔门店ID 字符串，为空表示可查看所有
        /// </summary>
        public string CanViewStores { get; set; }
        /// <summary>
        /// 是否显示门店选择组件
        /// </summary>
        /// <returns></returns>
        public bool ShowSelectStore()
        {
            if (string.IsNullOrEmpty(CanViewStores))
            {
                return true;
            }
            var viewStoresArray = CanViewStores.Split(',');
            if (viewStoresArray.Length > 1)
            {
                return true;
            }
            else {
                return false;  //只有一个门店值，只能查看当前门店,就默认当前账号门店，不显示
            }
        }

    }
}
