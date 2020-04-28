using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageResource : System.Dynamic.DynamicObject
    {
        private static global::Common.LanguageResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        internal static global::Common.LanguageResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::Common.LanguageResourceManager temp = new global::Common.LanguageResourceManager("Common.LanguageResource", typeof(LanguageResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }


        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            result = ResourceManager.GetString(binder.Name);
            return true;
        }
    }
}
