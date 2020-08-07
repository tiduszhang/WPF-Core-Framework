using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Common
{
    /// <summary>
    /// TCP服务端
    /// @author zhangsx
    /// @date 2017/04/12 11:18:19
    /// </summary>
    public class TcpService
    {
        /// <summary>
        /// 隐藏构造
        /// </summary>
        private TcpService()
        {

        }

        /// <summary>
        /// TCP监听服务
        /// </summary>
        private static TcpListener tcpListener = null;

        /// <summary>
        /// 接收到消息
        /// </summary>
        public event Action<Message> ReceiveMessage;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 结束标记
        /// </summary>
        public int? LineOff { get; set; }

        /// <summary>
        /// 开启TCP监听
        /// </summary>
        public void Acceptor()
        {
            if (tcpListener == null)
            {
                tcpListener = new TcpListener(IPAddress.Any, Port);
                tcpListener.Start(200);
                IAsyncResult result = tcpListener.BeginAcceptTcpClient(new AsyncCallback(Acceptor), tcpListener);
            }
        }

        /// <summary>
        /// 开始监听，启用监听时使用，用于设置端口号开启服务。
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="port"></param>
        public static void Start(Encoding encoding, int port = 12333)
        {
            var tcpService = GetInstence();
            tcpService.Port = port;
            tcpService.Acceptor();
        }

        /// <summary>
        /// 
        /// </summary>
        private static TcpService tcpService = null;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static TcpService GetInstence()
        {
            if (tcpService == null)
            {
                tcpService = new TcpService();
            }
            return tcpService;
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        private void Disposable()
        {
            if (tcpListener != null)
            {
                tcpListener.Stop();
            }
            tcpListener = null;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            var tcpService = GetInstence();
            tcpService.Disposable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        private void Acceptor(IAsyncResult o)
        {
            TcpListener server = o.AsyncState as TcpListener;
            try
            {
                TcpClient client = server.EndAcceptTcpClient(o);
                System.Threading.ThreadPool.QueueUserWorkItem(obj =>
                {
                    var ns = client.GetStream();

                    try
                    {
                        do
                        {
                            try
                            {
                                System.Threading.Thread.Sleep(1);
                                var message = new Message();

                                if (this.LineOff == null)
                                {
                                    this.LineOff = (int)'\n';
                                }

                                var iChar = -1;

                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    while ((iChar = ns.ReadByte()) > 0)
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
                                message.NetworkStream = ns;
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
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        } while (!((client.Client.Poll(500, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected));

                        ("客户端<" + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + ">已断开！").WriteToLog(log4net.Core.Level.Info);
                    }
                    catch (Exception ex)
                    {
                        ("服务器监听异常，异常信息：" + ex.ToString()).WriteToLog(log4net.Core.Level.Error);
                    }
                    finally
                    {
                        //ms.Close();
                        //ms.Dispose();
                        //ms = null; 

                        ns.Close();
                        ns.Dispose();
                        ns = null;
                        Close(client);
                    }
                });
            }
            catch (Exception ex)
            {
                ("服务器监听异常，异常信息：" + ex.ToString()).WriteToLog(log4net.Core.Level.Error);
            }
            finally
            {
                try
                {
                    IAsyncResult result = server.BeginAcceptTcpClient(new AsyncCallback(Acceptor), server);
                }
                catch (Exception ex)
                {
                    ("服务器监听异常停止，异常信息：" + ex.ToString()).WriteToLog(log4net.Core.Level.Error);
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="client"></param>
        private void Close(TcpClient client)
        {
            if (client.Connected)
            {
                client.Client.Shutdown(SocketShutdown.Both);
            }
            client.Client.Close();
            client.Close();
            client = null;
        }
    }
}
