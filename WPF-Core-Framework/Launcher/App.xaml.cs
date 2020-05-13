using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
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
        public System.Threading.EventWaitHandle ProgramStarted { get; set; }

        private const int SW_SHOWNOMAL = 1;

        ///<summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWindow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);


        public App()
        {
            this.InitializeComponent();

            "App初始化完成".WriteToLog(log4net.Core.Level.Info, "AA");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (StartOnece())//判断已经启动一次则唤醒程序退出第二次启动。
            {
                return;
            }

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //设置当前程序集
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            "App开始启动".WriteToLog(log4net.Core.Level.Info, "BB");
            base.OnStartup(e);
            "App完成启动".WriteToLog(log4net.Core.Level.Info, "BB");

            Task.Factory.StartNew(() =>
            { 
                do
                {
                    "APP task AA 正在写入日志测试！".WriteToLog(log4net.Core.Level.Info, "AA");
                    "APP task BB 正在写入日志测试！".WriteToLog(log4net.Core.Level.Info, "BB");
                    System.Threading.Thread.Sleep(1);
                } while (true); 
            }); 
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
                //MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
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

        /// <summary>
        /// 只打开一个进程
        /// </summary>
        protected bool StartOnece()
        {
            string mutexName = "32283F61-EC4D-43B1-9C44-40280D5854DD";

            ProgramStarted = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, mutexName, out var createNew);

            if (!createNew)
            {
                try
                {
                    var processes = System.Diagnostics.Process.GetProcessesByName(WorkPath.AssemblyName);
                    if (!processes.Any())
                    {
                        MessageBox.Show("已经启动了XXX", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        foreach (System.Diagnostics.Process process in processes)
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_SHOWNOMAL);
                            SetForegroundWindow(process.MainWindowHandle);
                            SwitchToThisWindow(process.MainWindowHandle, true);
                        }
                    }
                }
                catch (Exception exception)
                {
                    exception.ToString();
                    // Logger.Error(exception, "唤起已启动进程时出错");
                }

                App.Current.Shutdown();
                Environment.Exit(-1);
                return true;
            }
            return false;
        }

    }
}
