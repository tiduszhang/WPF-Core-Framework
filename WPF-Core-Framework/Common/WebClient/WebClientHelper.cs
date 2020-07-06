using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// <returns></returns>
        public static WebClient CreatedFastWebClient()
        {
            var webClient = CreatedWebClient() as WebClientCore;
            webClient.Timeout = 1000;
            return webClient;
        }

        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <returns></returns>
        public static WebClient CreatedWebClient()
        {
            var webClient = new WebClientCore();

            //设置不验证 
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((a, b, c, d) =>
            {
                return true;
            });

            webClient.Encoding = Encoding.UTF8;
            return webClient;
        }


        /// <summary>
        /// 创建WebClient对象(断点续接 ：  一般用于下载文件)
        /// </summary>
        /// <param name="seek"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(long seek)
        {
            var webClient = CreatedWebClient() as WebClientCore;

            //设置不验证 
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((a, b, c, d) =>
            {
                return true;
            });

            webClient.Seek = seek;
            webClient.Encoding = Encoding.UTF8;
            return webClient;
        }

        /// <summary>
        /// 创建WebClient对象
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static WebClient CreatedWebClient(string user, string pass)
        {
            var webClient = CreatedWebClient();

            ServicePointManager.ServerCertificateValidationCallback = null;

            webClient.Credentials = new NetworkCredential(user, pass);
            return webClient;
        }

        public static void test()
        {
            Task.Factory.StartNew(() =>
            {

                var filePath = "D:/temp/down/s1.mov";
                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var webClient = CreatedWebClient(fileStream.Length);
                var readStream = webClient.OpenRead("https://sylvan.apple.com/Aerials/2x/Videos/comp_GMT312_162NC_139M_1041_AFRICA_NIGHT_v14_SDR_FINAL_20180706_SDR_2K_HEVC.mov");

                if (fileStream.Length > 0)
                {
                    fileStream.Seek(fileStream.Length, SeekOrigin.Current);
                }

                byte[] buffer = new byte[4096];
                int i = 0;
                int l = 0;
                int b = 0;
                try
                {
                    while ((l = readStream.Read(buffer)) > 0)
                    {
                        i = i + l;
                        fileStream.Write(buffer, 0, l);
                        Array.Clear(buffer, 0, buffer.Length);

                        if (b++ % 10 == 0)
                        {
                            fileStream.Flush(true);
                        }
                    }

                    fileStream.Flush(true);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                readStream.Close();
                fileStream.Close();
                Thread.Sleep(1000);
                File.Move(filePath, "D:/temp/down/comp_GMT312_162NC_139M_1041_AFRICA_NIGHT_v14_SDR_FINAL_20180706_SDR_2K_HEVC.mov");
            });
        }

    }
}
