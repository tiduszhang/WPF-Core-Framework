using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Common
{
    /// <summary>
    /// WebClient帮助类
    /// </summary>
    public static class WebClientHelper
    {
        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(this Uri value)
        {
            var webClient = new WebClientCore();
            webClient.Timeout = 1000;
            webClient.Encoding = Encoding.UTF8;
            return webClient;
        }


        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(this string value)
        {
            return new Uri(value).CreatedWebClient();
        }

        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(this Uri value, string user, string pass)
        {
            var webClient = value.CreatedWebClient();
            webClient.Credentials = new NetworkCredential(user, pass);
            return webClient;
        }

        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(this string value, string user, string pass)
        {
            return new Uri(value).CreatedWebClient(user, pass);
        }


    }
}
