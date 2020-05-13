using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHepler
    {
        /// <summary>
        /// 将对象保存成JSON格式的文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void SaveJsonFile(this Object value, String path, String fileName)
        {
       
            if(!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            var filePath = System.IO.Path.Combine(path, fileName);

            System.IO.File.WriteAllText(filePath, value.ToJsonString());
        }

        /// <summary>
        /// 读取JSON格式文件并转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">文件夹</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static T LoadJsonFile<T>(this string value , string fileName) where T : class
        {
            T obj = default(T);

            if (!System.IO.Directory.Exists(value))
            {
                System.IO.Directory.CreateDirectory(value);
            }

            var filePath = System.IO.Path.Combine(value, fileName);

            if (System.IO.File.Exists(filePath))
            {
                obj = System.IO.File.ReadAllText(filePath).ToObject<T>();
            }

            return obj;
        }

    }
}
