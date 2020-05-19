using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVM
{
    /// <summary>
    /// 属性更改通知
    /// </summary>
    public class NotifyPropertyBase : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary> 
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性修改
        /// </summary>
        /// <param name="propertyName"> </param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 属性和属性值
        /// </summary>
        private IDictionary<string, object> _ValueDictionary = new Dictionary<string, object>();

        private List<object> _list { get; set; }

        public NotifyPropertyBase(string json)
        {
            var parse = fastJSON.JSON.Parse(json);

            if (parse is IDictionary<string, object>)
                _ValueDictionary = (IDictionary<string, object>)parse;
            else
                _list = (List<object>)parse;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NotifyPropertyBase()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dictionary"></param>
        public NotifyPropertyBase(IDictionary<string, object> dictionary)
        {
            if (dictionary != null)
            {
                //dictionary = new Dictionary<string, object>();
                _ValueDictionary = dictionary;
            }
        }

        #region 根据属性名得到属性值

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this._ValueDictionary.FirstOrDefault(dictionary => dictionary.Key.ToLowerInvariant() == binder.Name.ToLowerInvariant()).Value;

            //if (result is IDictionary<string, object>)
            //{
            //    result = new NotifyPropertyBase(result as IDictionary<string, object>);
            //}
            //else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
            //{
            //    result = new List<NotifyPropertyBase>((result as ArrayList).Cast<IDictionary<string, object>>().Select(x => new NotifyPropertyBase(x)));
            //}
            //else if (result is ArrayList)
            //{
            //    result = new List<NotifyPropertyBase>((result as ArrayList).Cast<NotifyPropertyBase>());
            //}

            //return base.TryGetMember(binder, out result);

            if (result is IDictionary<string, object>)
            {
                result = new NotifyPropertyBase(result as IDictionary<string, object>);
            }
            else if (result is List<object>)
            {
                List<object> list = new List<object>();
                foreach (object item in (List<object>)result)
                {
                    if (item is IDictionary<string, object>)
                        list.Add(new NotifyPropertyBase(item as IDictionary<string, object>));
                    else
                        list.Add(item);
                }
                result = list;
            }

            return true;
        }

        /// <summary>
        /// 属性访问
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        public virtual T GetValue<T>([CallerMemberName]string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("invalid " + propertyName);
            }
            object _propertyValue;
            if (_ValueDictionary.TryGetValue(propertyName.ToLowerInvariant(), out _propertyValue) && _propertyValue != null)
            {
                return (T)_propertyValue;
            }

            //获取属性
            var property = this.GetType().GetProperty(propertyName);
            var attributes = property.GetCustomAttributes(false);
            if (attributes == null || attributes.Length <= 0)
            {
                return (T)_propertyValue;
            }
            var attribute = attributes.FirstOrDefault(o => o is AutoConstructionAttribute);

            if (attribute == null)
            {
                return (T)_propertyValue;
            }

            AutoConstructionAttribute autoConstructionAttribute = attribute as AutoConstructionAttribute;

            //获取属性构造类型
            Type objType = autoConstructionAttribute.Type;
            if (objType == null)
            {
                objType = typeof(T);
            }

            if (autoConstructionAttribute.IsSingle)//单例
            {
                lock (AutoConstructionAttribute.ValueDictionary)
                {
                    if (AutoConstructionAttribute.ValueDictionary.TryGetValue(objType, out _propertyValue) && _propertyValue != null)
                    {
                        _ValueDictionary[propertyName.ToLowerInvariant()] = _propertyValue;
                        return (T)_propertyValue;
                    }
                }
            }

            //获取属性构造时使用的参数
            object args = autoConstructionAttribute.args;
            try
            {
                if (args == null)//自动构造
                {
                    _propertyValue = Activator.CreateInstance(objType);
                }
                else
                {
                    _propertyValue = Activator.CreateInstance(objType, args);
                } 
                _ValueDictionary[propertyName.ToLowerInvariant()] = _propertyValue;

                if (autoConstructionAttribute.IsSingle)//单例
                {
                    lock (AutoConstructionAttribute.ValueDictionary)
                    {
                        AutoConstructionAttribute.ValueDictionary[objType] = _propertyValue;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString().WriteToLog(log4net.Core.Level.Debug);
            }

            return (T)_propertyValue;
        }

        #endregion 根据属性名得到属性值

        #region 设置指定的属性值

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var data = this._ValueDictionary.FirstOrDefault(dictionary => dictionary.Key.ToLowerInvariant() == binder.Name.ToLowerInvariant());

            if (!String.IsNullOrWhiteSpace(data.Key.ToLowerInvariant()))
            {
                this._ValueDictionary.Remove(data.Key.ToLowerInvariant());
            }

            this._ValueDictionary[binder.Name.ToLowerInvariant()] = value;

            OnPropertyChanged(binder.Name);
            //return base.TrySetMember(binder, value);
            return true;
        }

        /// <summary>
        /// 属性访问
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="value"> </param>
        /// <param name="propertyName"> </param>
        public virtual void SetValue<T>(T value, [CallerMemberName]string propertyName = "")
        {
            if (!_ValueDictionary.ContainsKey(propertyName.ToLowerInvariant()) || _ValueDictionary[propertyName.ToLowerInvariant()] != (object)value)
            {
                _ValueDictionary[propertyName.ToLowerInvariant()] = value;
            }
            OnPropertyChanged(propertyName);
        }

        #endregion 设置指定的属性值

    }
}