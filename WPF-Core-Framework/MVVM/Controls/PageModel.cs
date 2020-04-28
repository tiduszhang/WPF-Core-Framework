using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVVM.Controls
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class PageModel : NotifyBaseModel
    {
        /// <summary>
        /// 首页
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "首页", ResourceType = typeof(Common.LanguageResource), ShortName = "First")]
        public bool First
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "末页", ResourceType = typeof(Common.LanguageResource), ShortName = "Last")]
        public bool Last
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "上一页", ResourceType = typeof(Common.LanguageResource), ShortName = "Prev")]
        public bool Prev
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "下一页", ResourceType = typeof(Common.LanguageResource), ShortName = "Next")]
        public bool Next
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 每页显示大小
        /// </summary> 
        public int PageSize
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 页码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "      第", ResourceType = typeof(Common.LanguageResource), ShortName = "PageIndex")]
        public int PageIndex
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 总条数
        /// </summary> 
        public int TotalCount
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "页", ResourceType = typeof(Common.LanguageResource), ShortName = "TotalPageCount")]
        public int TotalPageCount
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 分页文字信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "共{0}条  {1}/{2}页", ResourceType = typeof(Common.LanguageResource), ShortName = "PageInfo")]
        public string PageInfo
        {
            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 可以向前翻页
        /// </summary>
        public bool CanPrev
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 可以向后翻页
        /// </summary>
        public bool CanNext
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 可以翻页
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "转到", ResourceType = typeof(Common.LanguageResource), ShortName = "Go")]
        public bool CanGo
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 当进行翻页时执行
        /// </summary>
        public event EventPageingHandler ChangePageEvent;

        /// <summary>
        /// 绑定方法
        /// </summary>
        public virtual void ChangePage()
        {
            ChangePaging();

            //执行分页事件
            this.ChangePageEvent?.Invoke(new EventPagingArg()
            {
                PageModel = this
            });

            ChangePaged();
        }

        /// <summary>
        /// 分页完成
        /// </summary>
        public virtual void ChangePaged()
        {
            if (this.PageSize > 0)
            {
                //计算总页数
                this.TotalPageCount = (this.TotalCount / this.PageSize) + (this.TotalCount % this.PageSize > 0 ? 1 : 0);
            }

            this.CanGo = true;
            this.CanPrev = true;
            this.CanNext = true;

            if (this.PageIndex <= 1)
            {
                this.CanPrev = false;
            }
            if (this.PageIndex >= this.TotalPageCount)
            {
                this.CanNext = false;
            }

            //设置分页按钮
            if (this.TotalPageCount <= 1)
            {
                this.CanGo = false;
                this.CanPrev = false;
                this.CanNext = false;
            }

            //显示分页信息
            this.PageInfo = String.Format(this.DisplayName["PageInfo"], this.TotalCount, this.PageIndex, this.TotalPageCount);
            //  this.PageInfo = "共" + this.TotalCount + "条  " + this.PageIndex + "/" + this.TotalPageCount + "页";
        }

        /// <summary>
        /// 分页开始
        /// </summary>
        public virtual void ChangePaging()
        {
            if (this.PageIndex <= 0)
            {
                this.PageIndex = 0;
            }
            else if (this.PageIndex >= this.TotalPageCount)
            {
                this.PageIndex = this.TotalPageCount;
            }
        }
    }

    /// <summary>
    /// 分页事件委托
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void EventPageingHandler(EventPagingArg e);

    /// <summary>
    /// 分页参数
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        /// <summary>
        /// 分页实体参数
        /// </summary>
        public PageModel PageModel { get; set; }
    }

    /// <summary>
    /// 分页事件委托
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void EventPageingHandler<T>(EventPagingArg<T> e);

    /// <summary>
    /// 分页参数
    /// </summary>
    public class EventPagingArg<T> : EventArgs
    {
        /// <summary>
        /// 分页实体参数
        /// </summary>
        public PageViewModel<T> PageModel { get; set; }
    }

    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageViewModel<T> : PageModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageViewModel()
        {
            ObservableCollectionObject = new ObservableCollection<T>();
        }

        /// <summary>
        /// 展示列表数据
        /// </summary>
        public ObservableCollection<T> ObservableCollectionObject
        {
            get
            {
                return this.GetValue<ObservableCollection<T>>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 当进行翻页时执行
        /// </summary>
        public new event EventPageingHandler<T> ChangePageEvent;

        /// <summary>
        /// 分页绑定
        /// </summary>
        public override void ChangePage()
        {
            base.ChangePaging();

            //调用执行查询方法，并接受返回值
            List<T> QueryData = this.ChangePageFun?.Invoke();
            //填充数据
            Fill(QueryData);

            if (ObservableCollectionObject.Count > 0 && this.PageIndex <= 0)
            {
                this.PageIndex = 1;
            }

            //执行分页事件
            this.ChangePageEvent?.Invoke(new EventPagingArg<T>()
            {
                PageModel = this
            });

            if (this.TotalCount <= 0)
            {
                this.TotalCount = ObservableCollectionObject.Count;
            }
            base.ChangePaged();
        }

        /// <summary>
        /// 翻页查询扩展方法
        /// </summary>
        public Func<List<T>> ChangePageFun;

        /// <summary>
        /// 分页绑定
        /// </summary>
        /// <param name="fun">翻页查询扩展方法</param>
        public virtual void ChangePage(Func<List<T>> fun)
        {
            ChangePageFun = fun;

            ChangePage();
        }

        /// <summary>
        /// 将数据转换成列表
        /// </summary>
        /// <returns></returns>
        public virtual List<T> ToList()
        {
            return ObservableCollectionObject.ToList();
        }

        /// <summary>
        /// 填充数据到绑定结果集中
        /// </summary>
        private void Fill(List<T> QueryData)
        {
            if (ObservableCollectionObject.Count > 0)
            {
                for (int i = ObservableCollectionObject.Count - 1; ObservableCollectionObject.Count == 0; i--)
                {
                    ObservableCollectionObject.Remove(ObservableCollectionObject[i]);
                }
            }

            if (QueryData == null || QueryData.Count <= 0)
            {
                return;
            }

            QueryData.ForEach(o =>
            {
                ObservableCollectionObject.Add(o);
            });
        }
    }
}