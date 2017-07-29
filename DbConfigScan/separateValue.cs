using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConfigScan
{
    public class SeparateValue
    {
        public SeparateValue(string combinValue)
        {
            if (string.IsNullOrEmpty(combinValue))
            {
                throw new ArgumentException("不能使用空字符串初始化", nameof(combinValue));
            }
            var list = GetKeyPair(combinValue);
            SetValue(list);
        }

        private IEnumerable<KeyValuePair<string,string>> GetKeyPair(string combinValue)
        {
            var keyValuePairStr = combinValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in keyValuePairStr)
            {
                var x = item.Split(new[] { '=' }, 2);
                var key = x[0];
                var value = string.Empty;
                if (x.Length == 2)
                {
                    value = x[1];
                }
                result.Add(new KeyValuePair<string, string>(key, value));
            }
            return result;
        }
        private void SetValue(IEnumerable<KeyValuePair<string,string>> values)
        {
            DbName = values.Where(x => TextEq(x.Key, "Database", "Initial Catalog")).FirstOrDefault().Value;
            User = values.Where(x => TextEq(x.Key, "User", "UId", "UserId","User Id","UserName","User Name")).FirstOrDefault().Value;
            Pwd = values.Where(x => TextEq(x.Key, "Password", "PWD")).FirstOrDefault().Value;
        }
        private bool TextEq(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }
        private bool TextEq(string str1,params string[] strs)
        {
            if(strs==null || strs.Length == 0)
            {
                return false;
            }
            foreach(var item in strs)
            {
                if (TextEq(str1, item))
                {
                    return true;
                }
            }
            return false;
        }

        public SeparateValue(string dbName,string user,string pwd)
        {
            DbName = dbName?.Trim() ?? string.Empty;
            User = user?.Trim() ?? string.Empty;
            Pwd = pwd?.Trim() ?? string.Empty;
        }

        public string DbName { get; private set; }
        public string User { get; private set; }
        public string Pwd { get; private set; }
    }
}
