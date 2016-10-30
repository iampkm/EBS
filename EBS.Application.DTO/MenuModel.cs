using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
namespace EBS.Application.DTO
{
    public class MenuModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int UrlType { get; set; }

        public void Validate()
        {
            MenuModelValidator validator = new MenuModelValidator();
            ValidationResult result = validator.Validate(this);
            if (!result.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                result.Errors.ToList().ForEach(error => sb.Append(error.ErrorMessage + ";"));
                throw new Exception(sb.ToString());
            }
        }

    }
    public class MenuModelValidator : AbstractValidator<MenuModel>
    {
        //.WithLocalizedMessage(() => "请输入{PropertyName}")
        public MenuModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithLocalizedName(() => "菜单").WithLocalizedMessage(() => "请输入{PropertyName}");
            // RuleFor(m => m.Password).NotEmpty().WithLocalizedName(() => "密码").WithLocalizedMessage(() => "请输入{PropertyName}");
        }
    }
}
