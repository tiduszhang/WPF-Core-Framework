using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Udp服务
    /// </summary>
    public class UdpService
    {
        /// <summary>
        /// 隐藏构造
        /// </summary>
        private UdpService()
        {

        }

        /// <summary>
        /// Udp监听服务
        /// </summary>
        private static UdpClient udpListener = null;

        /// <summary>
        /// 接收到消息
        /// </summary>
        public event Action<Message> ReceiveMessage;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }


        /// <summary>
        /// 开启TCP监听
        /// </summary>
        public void Acceptor()
        {
            if (udpListener == null)
            {
                udpListener = new UdpClient(new IPEndPoint(IPAddress.Any, this.Port));
                Receive();
            }
        }

        /// <summary>
        /// 开始监听，启用监听时使用，用于设置端口号开启服务。
        /// </summary>
        /// <param name="port"></param>
        public static void Start(int port = 12334)
        {
            var udpService = GetInstence();
            udpService.Port = port;
            udpService.Acceptor();
        }

        /// <summary>
        /// 
        /// </summary>
        private static UdpService udpService = null;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static UdpService GetInstence()
        {
            if (udpService == null)
            {
                udpService = new UdpService();
            }
            return udpService;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            var udpService = GetInstence();
            udpService.Disposable();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        private void Disposable()
        {
            if (udpListener != null)
            {
                udpListener.Close();
            }
            udpListener = null;
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="client"></param>
        private void Close(UdpClient client)
        {
            client.Client.Shutdown(SocketShutdown.Both);
            client.Client.Close();
            client.Close();
            client = null;
        }


        private void Receive()
        { 
            udpListener.ReceiveAsync().ContinueWith(udpReceiveResult =>
            { 
                UdpReceiveResult receiveResult = udpReceiveResult.Result; 
                try
                {
                    string strMessage = receiveResult.Buffer.ConvertToUTF8String();
                    if (!String.IsNullOrWhiteSpace(strMessage))
                    {
                        Message message = new Message();
                        message.Content = strMessage;
                        message.UdpServer = udpListener;
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
