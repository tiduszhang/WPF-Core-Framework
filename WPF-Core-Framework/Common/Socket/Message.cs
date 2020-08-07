using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    /// <summary>
    /// 消息对象
    /// @author zhangsx
    /// @date 2017/04/12 11:18:19
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 消息内容JSON字符串，一般运行程序时指定的参数，比如打开浏览器时指定网址
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 目标IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 目标端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 远程主机
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; internal set; }

        /// <summary>
        /// Udp服务器
        /// </summary>
        public UdpClient UdpServer { get; internal set; }

        /// <summary>
        /// 远程主机写入
        /// </summary>
        public NetworkStream NetworkStream { get; internal set; }

        /// <summary>
        /// Tcp返回消息
        /// </summary>
        /// <param name="content"></param>
        public void TcpCallBackMessage(string content)
        {
            ("返回数据：" + content).WriteToLog(log4net.Core.Level.Info);

            NetworkStream.Write(content.ConvertToUTF8Bytes());
            NetworkStream.Flush();
        }

        /// <summary>
        /// Udp返回消息
        /// </summary>
        /// <param name="content"></param>
        public void UdpCallBackMessage(string content)
        {
            ("返回数据：" + content).WriteToLog(log4net.Core.Level.Info);

            if (!String.IsNullOrWhiteSpace(content))
            {
                byte[] bData = content.ConvertToUTF8Bytes();
                UdpServer.Send(bData, bData.Length, RemoteEndPoint);
            } 
        }


    }
}
