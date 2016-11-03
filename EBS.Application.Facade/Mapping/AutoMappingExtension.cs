using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using EBS.Application.DTO;
using EBS.Domain.Entity;
using System.Collections;

namespace EBS.Application.Facade.Mapping
{
   public static class AutoMappingExtension
    {
        static AutoMappingExtension() {
            //配置映射
        }

        /// <summary>
        /// 集合对集合
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            if (self == null)
                throw new ArgumentNullException();
            Mapper.Initialize(cfg => cfg.CreateMap(self.GetType(), typeof(TResult)));   
            return (List<TResult>)Mapper.Map(self, self.GetType(), typeof(List<TResult>));
        }
        /// <summary>
        /// 对象对对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TResult MapTo<TResult>(this object self)
        {
            if (self == null)
                throw new ArgumentNullException();
             Mapper.Initialize(cfg => cfg.CreateMap(self.GetType(), typeof(TResult)));           
           // Mapper.Map(self, self.GetType(), typeof(TResult));
            return (TResult)Mapper.Map(self, self.GetType(), typeof(TResult));
        }

    }
}
