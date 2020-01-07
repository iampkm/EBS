using EBS.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBS.Domain
{
   public class SettingsCollection:ISettings
    {
        List<Setting> _settings;
        public SettingsCollection(List<Setting> settings)
        {
            _settings = settings;
        }

        public Setting Get(string keyName)
        {
            return _settings.FirstOrDefault(n => n.KeyName == keyName);
        }

        public Dictionary<string, Setting> GetAll()
        {
            return _settings.ToDictionary(n=>n.KeyName);
        }

        public Setting GetByStartKey(string keyName)
        {
            return _settings.FirstOrDefault(n => n.KeyName.StartsWith(keyName));
        }
    }
}
