using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
namespace EBS.Infrastructure.IoC
{
   public interface IDependencyRegistrar
    {
       void Register(ContainerBuilder builder,ITypeFinder typeFinder);
    }
}
