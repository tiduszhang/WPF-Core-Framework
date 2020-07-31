using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 消息类 TCP扩展
    /// </summary>
    public static class MessageTcpExpansion
    {
        /// <summary>
        /// 注册监听任务
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        public static void RegisterTcpReceiveMessage(this object value, Action<Message> func)
        {
            TcpService tcpService = TcpService.GetInstence();
            tcpService.ReceiveMessage += func;
        }

        /// <summary>
        /// 注销监听任务
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        public static void UnRegisterTcpReceiveMessage(this object value, Action<Message> func)
        {
            TcpService tcpService = TcpService.GetInstence();
            tcpService.ReceiveMessage -= func;
        }


        /// <summary>
        /// 注册监听事件-客户端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void RegisterTcpClientReceiveMessage(this object value, Action<Message> action, string ip = "127.0.0.1", int port = 12333)
        {
            TcpHost tcpHost = TcpHost.GetInstence(Encoding.UTF8, ip, port);
            tcpHost.ReceiveMessage += action;
        }

        /// <summary>
        /// 注册监听事件-客户端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void UnRegisterTcpClientReceiveMessage(this object value, Action<Message> action, string ip = "127.0.0.1", int port = 12333)
        {
            TcpHost tcpHost = TcpHost.GetInstence(Encoding.UTF8, ip, port);
            tcpHost.ReceiveMessage -= action;
        }
        
    }
}
