using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{
    public enum LifeTime
    {
        /// <summary>
        /// 单例
        /// [每次获取都是同一个实例]
        /// </summary>
        Singleton,
        /// <summary>
        /// 作用域
        /// [同一作用域内获取都是同一个实例
        /// 在ASP.NET Core里，每次请求相当于同一作用域]
        /// </summary>
        Scoped,
        /// <summary>
        /// 瞬态
        /// [每次获取都是新实例]
        /// </summary>
        Transient
    }
}
