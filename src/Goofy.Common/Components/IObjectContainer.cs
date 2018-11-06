using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{
    public interface IObjectContainer
    {

        void Build();

        IObjectContainer Register(Type serviceType, Type implementationType, LifeTime lifetime = LifeTime.Singleton);

        object Resolve(Type serviceType);

    }
}
