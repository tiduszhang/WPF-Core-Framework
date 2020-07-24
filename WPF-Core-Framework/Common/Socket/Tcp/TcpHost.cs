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
        /// 网络流对象-写入
        /// </summary>
        StreamWriter TcpWriter { get; set; }

        /// <summary>
        /// 网络流对象-读取
        /// </summary>
        StreamReader TcpReader { get; set; }

        /// <summary>
        /// 字符集
        /// </summary>
        public Encoding Encoding { get; set; }

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
        /// <param name="encoding"></param
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static TcpHost GetInstence(Encoding encoding, string ip = "127.0.0.1", int port = 12333)
        {
            if(tcpHosts == null)
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
                tcpHost.Encoding = encoding;
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

            if (TcpWriter != null)
            {
                try
                {
                    TcpWriter.Close();

                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            TcpWriter = null;

            if (TcpReader != null)
            {
                try
                {
                    TcpReader.Close();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            TcpReader = null;
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
                    var NetworkStream = tcpClient.GetStream();
                    NetworkStream.ReadTimeout = 5 * 1000;
                    TcpWriter = new StreamWriter(NetworkStream);

                    if(this.Encoding == null)
                    {
                        this.Encoding = Encoding.UTF8;
                    }

                    TcpReader = new StreamReader(NetworkStream, this.Encoding);
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
            if (TcpWriter != null)
            {
                ("从客户端向服务器发送数据：" + message.Content).WriteToLog(log4net.Core.Level.Info);
                //var bData = (message.Content + Environment.NewLine).ConvertToBytes();
                TcpWriter.WriteLine(message.Content);
                TcpWriter.Flush();
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
                                   string data = "";
                                   if (LineOff == null)
                                   {
                                       data = TcpReader.ReadLine();
                                   }
                                   else
                                   {
                                       StringBuilder stringBuilder = new StringBuilder();
                                       var iChar = -1;

                                       while ((iChar = TcpReader.Read()) > 0)
                                       {
                                           stringBuilder.Append((Char)iChar);
                                           if (iChar == this.LineOff)
                                           {
                                               break;
                                           }
                                       }
                                       data = stringBuilder.ToString();
                                   }

                                   if (!String.IsNullOrWhiteSpace(data))
                                   {
                                       Message message = new Message
                                       {
                                           TcpWriter = TcpWriter,
                                           Content = data,
                                           IP = IP,
                                           Port = Port
                                       };

                                       ("从服务器中接收数据：" + message.Content).WriteToLog(log4net.Core.Level.Info);
                                       ReceiveMessage(message);
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
