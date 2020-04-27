using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, log4net.ILog> _logs = null;



        /// <summary>
        /// 写入日志 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <param name="mark"></param>
        public static void WriteToLog(this string value, log4net.Core.Level level, string mark = "")
        {
            log4net.GlobalContext.Properties["LogUrl"] = WorkPath.ApplicationWorkPath + "\\logs\\" + mark;

            string config = WorkPath.ExecPath + @"Config\Log4net.config";

            if (_logs == null)
            {
                _logs = new Dictionary<string, log4net.ILog>();
            }


            log4net.ILog log = null;

            lock (_logs)
            {
                if (!_logs.TryGetValue(mark.ToLowerInvariant(), out log))
                {
                    log4net.Repository.ILoggerRepository loggerRepository = log4net.LogManager.CreateRepository(mark);
                    if (File.Exists(config))
                    {
                        log4net.Config.XmlConfigurator.Configure(loggerRepository, new System.IO.FileInfo(config));
                    }

                    log = log4net.LogManager.GetLogger(mark, mark);
                    //log = new log4net.Core.LogImpl(loggerRepository.GetLogger(""));  //log4net.LogManager.GetLogger(mark, mark);
                    _logs.Add(mark.ToLowerInvariant(), log);

                }
                var appenders = log.Logger.Repository.GetAppenders();
            }


            if (level == log4net.Core.Level.Debug)
            {
                log.Debug(value);
            }
            else if (level == log4net.Core.Level.Info)
            {
                log.Info(value);
            }
            else if (level == log4net.Core.Level.Error)
            {
                log.Error(value);
            }
            else if (level == log4net.Core.Level.Fatal)
            {
                log.Fatal(value);
            }
            else if (level == log4net.Core.Level.Warn)
            {
                log.Warn(value);
            }
            else
            {
                log.Info(value);
            }
            //try
            //{
            //    using (StreamWriter s = System.IO.File.AppendText(System.IO.Path.GetFullPath(Constant.ApplicationWorkPath + @"\Log.log")))
            //    {
            //        s.WriteLine(value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
        }
    }
}
