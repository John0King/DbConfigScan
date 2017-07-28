using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;

namespace DbConfigScan
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");
            app.FullName = "数据库连接字符串扫描器";
            var registCmd = app.Command("regist", p =>
            {
                p.Description = "将程序注册到当前用户的环境变量中";
                p.OnExecute(() =>
                {
                    
                    var currentEnvPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                    Console.WriteLine("当前用户的环境变量-Path:");
                    Console.WriteLine(currentEnvPath);

                    var newEnvPath = $"{currentEnvPath};{AppContext.BaseDirectory}";
                    Environment.SetEnvironmentVariable("PATH", newEnvPath, EnvironmentVariableTarget.User);
                    Console.WriteLine("新的用户的环境变量-Path:");
                    Console.WriteLine(newEnvPath);
                    Console.WriteLine("已将当前程序注册到环境变量");
                    return 0;
                });
            });
            var unregistCmd = app.Command("unregist", p =>
            {
                p.Description = "将程序注册从当前用户的环境变量中移除";
                p.OnExecute(() =>
                {
                    var currentEnvPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                    Console.WriteLine("当前用户的环境变量-Path:");
                    Console.WriteLine(currentEnvPath);

                    var paths = currentEnvPath.Split(';');

                    var pathList = paths.Select(x => x.Trim())
                        .Where(x => !string.Equals(AppContext.BaseDirectory, x, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    var newEnvPath = string.Join(";", pathList);
                    Environment.SetEnvironmentVariable("PATH", newEnvPath, EnvironmentVariableTarget.User);
                    Console.WriteLine("新的用户的环境变量-Path:");
                    Console.WriteLine(newEnvPath);
                    Console.WriteLine("已将当前程序注册到环境变量");
                    return 0;
                });
            });
            var scanCmd = app.Command("scan", p =>
            {
                p.Description = "扫描配置文件";
                var dirOpt = p.Option("-dir", "扫描文件夹路径,开头使用 ./ 来表示当前路径", CommandOptionType.MultipleValue);
                var outOpt = p.Option("-out", "CSV文件输出路径,如C:/abc.csv,开头使用 ./ 来表示当前路径", CommandOptionType.MultipleValue);
                p.OnExecute(() =>
                {
                    if (dirOpt.HasValue() && outOpt.HasValue())
                    {
                        Console.WriteLine("正在扫描，请耐心等待.....");
                        ScanDb(dirOpt.Value(),outOpt.Value());
                        Console.WriteLine("扫描完成, 将结果放到了{0}",Path.GetFullPath(outOpt.Value()));
                    }
                    else
                    {
                        p.ShowHelp();
                    }
                    return 0;
                });
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            app.Execute(args);
        }

        static int ScanDb(string dir,string outdir )
        {
            Console.WriteLine("正在扫描web.config , 请稍后 .....");
            var sources = new List<ConfigValue>();
            var webconfig = new WebConfigScanner(dir);
            sources.AddRange(webconfig.Scan());

            Console.WriteLine("web.config 扫描完成");

            Console.WriteLine("正在扫描 appsettings.*.json , 请稍后  .....");
            var jsonScanner = new JsonScanner(dir);
            sources.AddRange(jsonScanner.Scan());

            Console.WriteLine("appsettings.*.json 扫描完成");

            Console.WriteLine("正在叫结果写入 CSV");
            CSV.WriteToCSV(sources, outdir);

            Console.WriteLine("写入完成");

            return 0;
        }
    }
}
