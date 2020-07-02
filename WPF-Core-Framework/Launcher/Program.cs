using Common;
using System;
using System.ServiceProcess; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    static class Program
    {
        /// <summary>
        /// 程序唯一标识
        /// </summary>
        static string mutexName = "78A97384-F4CB-4FBE-990B-8242DEF6AC45";

        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                //检查程序是否已经启动过一次
                var programStarted = ApplicationTools.StartOnece(mutexName);
                if (programStarted == null)//判断已经启动一次则唤醒程序退出第二次启动。
                {
                    return;
                }
                else
                {
                    var app = new App();
                    app.ProgramStarted = programStarted;
                    app.Run();
                }
            }


        }
    }
}
