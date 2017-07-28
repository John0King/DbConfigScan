using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace DbConfigScan
{
    public class CSV
    {
        public static IList<ConfigValue> ReadCSV(string path)
        {
            using(var reader = new StreamReader(path,Encoding.Default))
            {
                var csvReader = new CsvReader(reader);
                csvReader.Configuration.RegisterClassMap(new ConfigValueCsvMaper());
                return csvReader.GetRecords<ConfigValue>().ToList();
            }
        }

        public static void WriteToCSV(IEnumerable<ConfigValue> list,string path)
        {
            using (var writer = new StreamWriter(path, false, Encoding.Default))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.Configuration.RegisterClassMap(new ConfigValueCsvMaper());
                csvWriter.WriteRecords(list);
            }
            
        }
    }
}
