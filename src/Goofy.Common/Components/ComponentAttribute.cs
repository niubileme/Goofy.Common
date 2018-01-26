using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public LifeStyle LifeStyle { get; private set; }

        public ComponentAttribute() : this(LifeStyle.Singleton) { }

        public ComponentAttribute(LifeStyle lifeStyle)
        {
            LifeStyle = lifeStyle;
        }
    }

    public enum LifeStyle
    {
        /// <summary>
        /// 瞬态
        /// </summary>
        Transient,
        /// <summary>
        /// 单例
        /// </summary>
        Singleton
    }
}
