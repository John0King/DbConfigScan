using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConfigScan
{
    public class CSVCombin
    {
        private readonly IEnumerable<string> _files;
        public CSVCombin(IEnumerable<string> files)
        {
            _files = files;
        }


        public void WriteTo(string path)
        {
            var list = ReadAllConfigValue();

            var dbValues = ReadToDbValues(list);

            CSV.WriteDbConfigToCSV(dbValues, path);
        }

        private IList<ConfigValue> ReadAllConfigValue()
        {
            var list = new List<ConfigValue>();
            foreach (var file in _files)
            {
                list.AddRange(CSV.ReadCSV(file));
            }

            return list;
        }

        private IList<DbValue> ReadToDbValues(IEnumerable<ConfigValue> configValues)
        {
            var seprateVList = new List<SeparateValue>();
            foreach(var cv in configValues)
            {
                if (!string.IsNullOrWhiteSpace(cv.Connection))
                {
                    seprateVList.Add(new SeparateValue(cv.Connection));
                }
                
            }

            return seprateVList
                .Select(s => new DbValue { DbName = s.DbName, UserName = s.User, Password = s.Pwd })
                .Where(s=>s.HasValue)
                .Distinct()
                .ToList();
        }

    }
}
