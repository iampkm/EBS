using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using Autofac;
namespace EBS.Admin.Services
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer _container;

        public AutofacValidatorFactory(IContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            object instance;
            if (_container.TryResolve(validatorType, out instance))
            {
                return instance as IValidator;
            }
            return null;
        }
    }
}