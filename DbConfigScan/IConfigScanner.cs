using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConfigScan
{
    public interface IConfigScanner
    {
        IEnumerable<ConfigValue> Scan();
    }
}
