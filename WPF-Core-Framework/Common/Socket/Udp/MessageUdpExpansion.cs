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


    }
}
