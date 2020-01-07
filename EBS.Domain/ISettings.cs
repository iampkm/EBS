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

        Setting GetByStartKey(string keyName);

        Dictionary<string,Setting> GetAll();
    }
}
