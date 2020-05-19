using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM
{
    /// <summary>
    /// MVVM基类，实例化成dynamic类型，可以支持动态属性
    /// </summary>
    public class NotifyBaseModel : NotifyPropertyBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotifyBaseModel()
        {
            DisplayName.NotifyProperty = this;
            Description.NotifyProperty = this;
            Prompt.NotifyProperty = this;
            ShortName.NotifyProperty = this;
            ValidationError.NotifyProperty = this;

            this.ErrorMessage = "";
            this.IsValid = true;
        }

        /// <summary>
        /// DisplayNameData
        /// </summary>
        [AutoConstruction]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual DisplayNameData DisplayName
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<DisplayNameData>();
            }
        }

        /// <summary>
        /// ValidationErrorData
        /// </summary>
        [AutoConstruction]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ValidationErrorData ValidationError
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<ValidationErrorData>();
            }
        }

        /// <summary>
        /// ShortNameData
        /// </summary>
        [AutoConstruction]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ShortNameData ShortName
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<ShortNameData>();
            }
        }

        /// <summary>
        /// PromptData
        /// </summary>
        [AutoConstruction]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PromptData Prompt
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<PromptData>();
            }
        }

        /// <summary>
        /// DescriptionData
        /// </summary>
        [AutoConstruction]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual DescriptionData Description
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<DescriptionData>();
            }
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual bool IsValid
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<bool>();
            }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual string ErrorMessage
        {
            set
            {
                this.SetValue(value);
            }
            get
            {
                return this.GetValue<string>();
            }
        }

    }
}
