using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DbConfigScan
{
    public class JsonScanner : IConfigScanner
    {
        private readonly string _folder;

        public JsonScanner(string folder)
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
            var configs = RootDir.GetFiles("appsettings*.json", SearchOption.AllDirectories);
            var list = new List<ConfigValue>();
            foreach (var config in configs)
            {
                using (var stream = config.OpenRead())
                {
                    //try
                    //{
                    list.AddRange(SearchConfig(stream, config.FullName));
                    //}
                    //catch(Exception e)
                    //{
                    //    Console.Write(e);
                    //}
                }
            }
            return list; ;
        }

        public IEnumerable<ConfigValue> SearchConfig(Stream file, string fileName)
        {
            var reader = new StreamReader(file);

            var tokens = JToken.Parse(reader.ReadToEnd());
            if(tokens.Type == JTokenType.Object)
            {
                var rootObj = (JObject)tokens;
                var csSection = rootObj.Property("ConnectionStrings")?.Value;
                if(csSection == null)
                {
                    return new List<ConfigValue>();
                }
                var list = csSection.Children()
                    .OfType<JProperty>()
                    .Select(x => new ConfigValue { Source = fileName, Connection = x.Value.ToString() });
                return list;
            }
            return new List<ConfigValue>();
           
        }
    }
}
