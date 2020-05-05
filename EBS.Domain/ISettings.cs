using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBS.Domain.Entity;
using System.Collections;
namespace EBS.Domain
{
    public interface ISettings
    {
        Setting Get(string keyName);
        /// <summary>
        /// 根据key前缀查询单个配置
        /// </summary>
        /// <param name="startKey"></param>
        /// <returns></returns>
        Setting GetByStartKey(string startKey);

        /// <summary>
        /// 根据key前缀查询配置
        /// </summary>
        /// <param name="startKey"></param>
        /// <returns></returns>
        Dictionary<string, Setting> GetAll(string startKey);

        Dictionary<string,Setting> GetAll();
    }
}
