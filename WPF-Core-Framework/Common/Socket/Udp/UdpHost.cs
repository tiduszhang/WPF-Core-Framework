using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    /// <summary>
    /// UDP客户端主机(长连接)
    /// </summary>
    public class UdpHost
    {
        /// <summary>
        /// 不允许构造
        /// </summary>
        private UdpHost()
        {

        }

        /// <summary>
        /// 多例Host
        /// </summary>
        private static System.Collections.Specialized.HybridDictionary udpHosts = null;

        /// <summary>
        /// 单例TCP客户端
        /// </summary>
        private UdpClient udpClient = null;

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
        /// 关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static UdpHost GetInstence(string ip = "127.0.0.1", int port = 12333)
        {
            if (udpHosts == null)
            {
                udpHosts = new System.Collections.Specialized.HybridDictionary();
            }

            UdpHost udpHost = null;
            var key = ip + ":" + port;
            lock (udpHosts)
            {
                if (udpHosts.Contains(key))
                {
                    udpHost = udpHosts[key] as UdpHost;
                }
            }

            if (udpHost == null)
            {
                udpHost = new UdpHost();
                udpHost.IP = ip;
                udpHost.Port = port;
                udpHost.Receive();
                udpHost.Connection();
                lock (udpHosts)
                {
                    udpHosts.Add(key, udpHost);
                }
            }

            return udpHost;
        }


        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            if (udpHosts != null)
            {
                lock (udpHosts)
                {
                    foreach (UdpHost udpHost in udpHosts)
                    {
                        udpHost.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void Close()
        {
            isClose = true;
            if (udpClient != null)
            {
                try
                {
                    udpClient.Client.Shutdown(SocketShutdown.Both);
                    udpClient.Client.Close();
                    udpClient.Close();
                    udpClient = null;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            udpClient = null;
        }

        /// <summary>
        /// 连接服务端
        /// </summary>
        private void Connection()
        {
            try
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                }

                udpClient.Connect(new System.Net.IPEndPoint(IPAddress.Parse(IP), Port));
                isClose = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                Close();
            }
            Receive();
            GC.Collect();
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
            udpClient.Send(message.Content, message.Content.Length);
        }

        /// <summary>
        /// 开始一步监听
        /// </summary>
        private void Receive()
        {
            if (isClose)
            {
                return;
            }

            if (udpClient == null && !udpClient.Client.Connected)
            {
                Connection();
            }

            udpClient.ReceiveAsync().ContinueWith(udpReceiveResult =>
            {
                UdpReceiveResult receiveResult = udpReceiveResult.Result;
                try
                {
                    if (receiveResult.Buffer != null && receiveResult.Buffer.Length > 0)
                    {
                        Message message = new Message();
                        message.Content = receiveResult.Buffer;
                        message.UdpServer = udpClient;
                        message.RemoteEndPoint = receiveResult.RemoteEndPoint;
                        message.IP = receiveResult.RemoteEndPoint.Address.ToString();
                        message.Port = receiveResult.RemoteEndPoint.Port;

                        ("接收到数据：" + message.Content).WriteToLog(log4net.Core.Level.Info);
                        if (System.Windows.Application.Current != null)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (ReceiveMessage != null)
                                {
                                    ReceiveMessage(message);
                                }
                            }));
                        }
                        else
                        {
                            if (ReceiveMessage != null)
                            {
                                ReceiveMessage(message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString().WriteToLog(log4net.Core.Level.Error);
                }
                finally
                {
                    Receive();
                }
            });


        }


    }
}
