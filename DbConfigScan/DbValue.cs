using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConfigScan
{
    public class DbValue
    {
        public string DbName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is DbValue)
            {
                return this == obj as DbValue;
            }
            else
            {
                return false;
            }
        }

        public bool HasValue => !string.IsNullOrEmpty(DbName) || !string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password);

        public override int GetHashCode()
        {
            return $"{DbName?.ToLowerInvariant()},{UserName?.ToLowerInvariant()},{Password?.ToLowerInvariant()}".GetHashCode();
        }
        public static bool operator ==(DbValue obj1,DbValue obj2)
        {
            if (TextEq(obj1.DbName, obj2.DbName) && TextEq(obj1.UserName, obj2.UserName) && TextEq(obj1.Password, obj2.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool TextEq(string str1,string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(DbValue obj1,DbValue obj2)
        {
            return !(obj1 == obj2);
        }

    }

    public class DbValueCsvMaper : CsvClassMap<DbValue>
    {
        public DbValueCsvMaper()
        {
            Map(c => c.DbName).Name("数据库名称");
            Map(c => c.UserName).Name("用户名");
            Map(c => c.Password).Name("密码");
        }
    }
}
