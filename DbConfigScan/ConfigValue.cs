using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;

namespace DbConfigScan
{
    public class ConfigValue
    {
        /// <summary>
        /// 来源路径
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public string Connection { get; set; }
    }


    public class ConfigValueCsvMaper: CsvClassMap<ConfigValue>
    {
        public ConfigValueCsvMaper()
        {
            Map(c => c.Source).Name("来源");
            Map(c => c.Connection).Name("链接字符串");
        } 
    }
}
