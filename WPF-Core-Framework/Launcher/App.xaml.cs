using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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
        /// <summary>
        /// 程序启动事件
        /// </summary>
        public System.Threading.EventWaitHandle ProgramStarted { get; set; }
        /// <summary>
        /// 程序唯一标识
        /// </summary>
        string mutexName = "78A97384-F4CB-4FBE-990B-8242DEF6AC45";

        /// <summary>
        /// 构造函数
        /// </summary>
        public App()
        {
            this.InitializeComponent(); 
        }

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //检查程序是否已经启动过一次
            ProgramStarted = ApplicationTools.StartOnece(mutexName);
            if (ProgramStarted == null)//判断已经启动一次则唤醒程序退出第二次启动。
            {
                Application.Current.Shutdown();
                Environment.Exit(-1);
                return;
            }

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //设置当前程序集
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
             
            base.OnStartup(e); 
        }

        /// <summary>
        /// 异步处理县城异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                ("异步处理县城异常：《" + e.Exception.ToString() + "》").WriteToLog(log4net.Core.Level.Error);
                e.SetObserved();
            }
            catch (Exception ex)
            {
                ("不可恢复的异步处理县城异常：《" + ex.ToString() + "》").WriteToLog(log4net.Core.Level.Error); 
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    ("非UI线程全局异常" + exception.ToString()).WriteToLog(log4net.Core.Level.Error);
                }
            }
            catch (Exception ex)
            {
                ("不可恢复的非UI线程全局异常" + ex.ToString()).WriteToLog(log4net.Core.Level.Error);
            }
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                ("UI线程全局异常：《" + e.Exception.ToString() + "》").WriteToLog(log4net.Core.Level.Error);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                ("不可恢复的UI线程全局异常：《" + ex.ToString() + "》").WriteToLog(log4net.Core.Level.Error);
                //MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
            //MessageBox.Show(e.Exception.ToString());
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            ("程序退出").WriteToLog(log4net.Core.Level.Info);
            base.OnExit(e);
        }

        /// <summary>
        /// 操作系统操作退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            ("当用户结束时发生 Windows 通过注销或关闭操作系统的会话。").WriteToLog(log4net.Core.Level.Info);
            base.OnSessionEnding(e);
        }

    }
}
