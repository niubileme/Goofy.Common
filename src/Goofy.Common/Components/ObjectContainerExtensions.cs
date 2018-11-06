using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{
    public static class ObjectContainerExtensions
    {
        public static IObjectContainer Register(this IObjectContainer objectContainer, Type serviceType, LifeTime lifetime = LifeTime.Singleton)
        {
            return objectContainer.Register(serviceType, serviceType, lifetime);
        }

        public static IObjectContainer Register<TService, TImplementer>(this IObjectContainer objectContainer, LifeTime lifetime = LifeTime.Singleton) where TImplementer : TService
        {
            return objectContainer.Register(typeof(TService), typeof(TImplementer), lifetime);
        }

        public static IObjectContainer Register<TService>(this IObjectContainer objectContainer, LifeTime lifetime = LifeTime.Singleton)
        {
            return objectContainer.Register(typeof(TService), lifetime);
        }


        public static TService Resolve<TService>(this IObjectContainer objectContainer)
        {
            return (TService)objectContainer.Resolve(typeof(TService));
        }

    }
}
