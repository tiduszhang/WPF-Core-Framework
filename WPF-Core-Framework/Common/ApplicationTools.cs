using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Common
{
    /// <summary>
    /// 应用程序帮助类
    /// </summary>
    public static class ApplicationTools
    {

        /// <summary>
        /// 只打开一个进程
        /// </summary>
        public static System.Threading.EventWaitHandle StartOnece(string mutexName)
        {
            System.Threading.EventWaitHandle ProgramStarted = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, mutexName, out var createNew);

            if (!createNew)
            {
                try
                {
                    var processes = System.Diagnostics.Process.GetProcessesByName(WorkPath.AssemblyName);
                    if (!processes.Any())
                    {
                        //MessageBox.Show("程序已启动。", "提示提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        foreach (System.Diagnostics.Process process in processes)
                        {
                            Win32API.ShowWindowAsync(process.MainWindowHandle, Win32API.SW_SHOWNOMAL);
                            Win32API.SetForegroundWindow(process.MainWindowHandle);
                            Win32API.SwitchToThisWindow(process.MainWindowHandle, true);
                        }
                    }
                }
                catch (Exception exception)
                {
                    exception.ToString();
                    // Logger.Error(exception, "唤起已启动进程时出错");
                }

                //Application.Current.Shutdown();
                //Environment.Exit(-1);
                return null;
            }

            return ProgramStarted;
        }

    }
}
