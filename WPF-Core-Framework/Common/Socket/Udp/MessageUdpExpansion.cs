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
    /// 消息类 Udp 扩展
    /// </summary>
    public static class MessageUdpExpansion
    { 
        /// <summary>
        /// 注册监听任务UDP
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        public static void RegisterUdpReceiveMessage(this object value, Action<Message> func)
        {
            UdpService udpService = UdpService.GetInstence();
            udpService.ReceiveMessage += func;
        }

        /// <summary>
        /// 注销监听任务UDP
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        public static void UnRegisterUdpReceiveMessage(this object value, Action<Message> func)
        {
            UdpService udpService = UdpService.GetInstence();
            udpService.ReceiveMessage -= func;
        }


        /// <summary>
        /// 注册监听事件-客户端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void RegisterUdpClientReceiveMessage(this object value, Action<Message> action, string ip = "127.0.0.1", int port = 12333)
        {
            UdpHost tcpHost = UdpHost.GetInstence(ip, port);
            tcpHost.ReceiveMessage += action;
        }

        /// <summary>
        /// 注册监听事件-客户端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void UnRegisterUdpClientReceiveMessage(this object value, Action<Message> action, string ip = "127.0.0.1", int port = 12333)
        {
            UdpHost tcpHost = UdpHost.GetInstence(ip, port);
            tcpHost.ReceiveMessage -= action;
        }

    }
}
