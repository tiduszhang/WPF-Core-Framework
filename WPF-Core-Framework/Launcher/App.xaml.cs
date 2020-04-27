using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Common;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            "App初始化完成".WriteToLog(log4net.Core.Level.Info, "AA");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            "App开始启动".WriteToLog(log4net.Core.Level.Info, "BB");
            base.OnStartup(e);
            "App完成启动".WriteToLog(log4net.Core.Level.Info, "BB");

            Task.Factory.StartNew(() => {

                do
                {
                    "APP task AA 正在写入日志测试！".WriteToLog(log4net.Core.Level.Info, "AA");
                    "APP task BB 正在写入日志测试！".WriteToLog(log4net.Core.Level.Info, "BB");
                    System.Threading.Thread.Sleep(1);
                } while (true);
                    
            });
        }
    }
}
