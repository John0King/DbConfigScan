using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DbConfigScan
{
    public class WebConfigScanner:IConfigScanner
    {
        private readonly string _folder;

        public WebConfigScanner(string folder)
        {
            _folder = folder;

        }

        public IEnumerable<ConfigValue> Scan()
        {
            var RootDir = new DirectoryInfo(_folder);
            if (!RootDir.Exists)
            {
                throw new DirectoryNotFoundException(Path.GetFullPath(_folder));
            }
            var configs = RootDir.GetFiles("web.config", SearchOption.AllDirectories);
            var list = new List<ConfigValue>();
            foreach (var config in configs)
            {
                using (var stream = config.OpenRead())
                {
                    
                    list.AddRange(SearchConfig(stream,config.FullName));
                }
            }
            return list;
        }

        public IEnumerable<ConfigValue> SearchConfig(Stream file,string fileName)
        {
            var doc = XDocument.Load(file);
            var noods = doc.Descendants("connectionStrings")
                .Descendants("add")
                .Select(x => new ConfigValue { Source = fileName, Connection = x.Attribute("connectionString").Value });
            return noods;
        }
    }
}
