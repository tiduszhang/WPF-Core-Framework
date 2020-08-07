using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    /// <summary>
    /// tcp客户端主机(长连接)
    /// </summary>
    public class TcpHost
    {
        /// <summary>
        /// 不允许构造
        /// </summary>
        private TcpHost()
        {

        }

        /// <summary>
        /// 多例Host
        /// </summary>
        private static System.Collections.Specialized.HybridDictionary tcpHosts = null;

        /// <summary>
        /// 单例TCP客户端
        /// </summary>
        private TcpClient tcpClient = null;

        /// <summary>
        /// 接收到消息
        /// </summary>
        public event Action<Message> ReceiveMessage;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// 结束标记
        /// </summary>
        public int? LineOff { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        bool isClose = false;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static TcpHost GetInstence(string ip = "127.0.0.1", int port = 12333)
        {
            if (tcpHosts == null)
            {
                tcpHosts = new System.Collections.Specialized.HybridDictionary();
            }

            TcpHost tcpHost = null;
            var key = ip + ":" + port;
            lock (tcpHosts)
            {
                if (tcpHosts.Contains(key))
                {
                    tcpHost = tcpHosts[key] as TcpHost;
                }
            }

            if (tcpHost == null)
            {
                tcpHost = new TcpHost();
                tcpHost.IP = ip;
                tcpHost.Port = port;
                tcpHost.Connection();
                lock (tcpHosts)
                {
                    tcpHosts.Add(key, tcpHost);
                }
            }

            return tcpHost;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            if (tcpHosts != null)
            {
                lock (tcpHosts)
                {
                    foreach (TcpHost tcpHost in tcpHosts)
                    {
                        tcpHost.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            isClose = true;
            if (tcpClient != null)
            {
                try
                {
                    tcpClient.Close();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            tcpClient = null;

        }

        /// <summary>
        /// 开始连接
        /// </summary>
        private void Connection()
        {
            if (tcpClient == null)
            {
                try
                {
                    if (tcpClient == null)
                    {
                        tcpClient = new TcpClient();
                    }
                    tcpClient.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(IP), Port));
                    NetworkStream = tcpClient.GetStream();
                    NetworkStream.ReadTimeout = 5 * 1000;
                    isClose = false;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    tcpClient = null;
                }
                ReceiveAsync();
                GC.Collect();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(Message message)
        {
            if (isClose)
            {
                Connection();
            }
            if (NetworkStream != null)
            {
                ("从客户端向服务器发送数据：" + message.Content).WriteToLog(log4net.Core.Level.Info);
                //var bData = (message.Content + Environment.NewLine).ConvertToBytes();
                NetworkStream.Write(message.Content, 0, message.Content.Length);
                NetworkStream.Flush();
            }
        }

        /// <summary>
        /// 异步接收
        /// </summary>
        private void ReceiveAsync()
        {
            if (isClose)
            {
                return;
            }
            //  if (NetworkStream != null)
            //{
            System.Threading.ThreadPool.QueueUserWorkItem(obj =>
               {
                   try
                   {
                       while (!((tcpClient.Client.Poll(500, SelectMode.SelectRead) && (tcpClient.Client.Available == 0)) || !tcpClient.Client.Connected))
                       {
                           System.Threading.Thread.Sleep(500);
                           try
                           {
                               if (ReceiveMessage != null)
                               {
                                   var message = new Message();

                                   if (this.LineOff == null)
                                   {
                                       this.LineOff = (int)'\n';
                                   }

                                   var iChar = -1;

                                   using (MemoryStream memoryStream = new MemoryStream())
                                   {
                                       while ((iChar = NetworkStream.ReadByte()) > 0)
                                       {
                                           memoryStream.WriteByte((byte)iChar);
                                           if (iChar == this.LineOff)
                                           {
                                               break;
                                           }
                                       }
                                       message.Content = memoryStream.ToArray();
                                   }

                                   if (message.Content == null && message.Content.Length > 0)
                                   {
                                       continue;
                                   }

                                   ("接收到数据：" + message.Content).WriteToLog(log4net.Core.Level.Info);
                                   message.NetworkStream = NetworkStream;
                                   if (System.Threading.SynchronizationContext.Current != null)
                                   {

                                       System.Threading.SynchronizationContext.Current.Post(callback =>
                                       {
                                           ReceiveMessage?.Invoke(message);
                                       }, message);
                                   }
                                   else
                                   {
                                       ReceiveMessage?.Invoke(message);
                                   }
                               }
                           }
                           catch (Exception ex)
                           {
                               ex.ToString();
                           }
                       };
                   }
                   catch (Exception ex)
                   {
                       ex.ToString();
                   }

                   Close();
                   Connection();
               });
        }
    }
}
