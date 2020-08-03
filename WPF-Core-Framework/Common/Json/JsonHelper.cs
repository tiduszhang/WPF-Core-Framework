using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将字符串转换成动态JSON对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this string value)
        {
            return new Common.DynamicJson(value);
        }

        /// <summary>
        /// 将字符串转换成指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value) where T : class
        {
            return fastJSON.JSON.ToObject<T>(value, new fastJSON.JSONParameters()
            {
                UseExtensions = false,
                BlackListTypeChecking = false,
                EnableAnonymousTypes = false,
                UsingGlobalTypes = false,
                IgnoreAttributes = new List<Type>()
                {
                    typeof(System.Text.Json.Serialization.JsonIgnoreAttribute)
                }
            });
        }

        /// <summary>
        /// 将对象转换成JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj) where T : class
        {
            return fastJSON.JSON.ToJSON(obj, new fastJSON.JSONParameters()
            {
                UseExtensions = false,
                BlackListTypeChecking = false,
                EnableAnonymousTypes = false,
                UsingGlobalTypes = false,
                IgnoreAttributes = new List<Type>()
                {
                    typeof(System.Text.Json.Serialization.JsonIgnoreAttribute)
                }
            });
        }
    }
}
