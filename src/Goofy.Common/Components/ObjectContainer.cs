using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{
    public class ObjectContainer
    {

        public static IObjectContainer Current { get; private set; }


        public static void SetContainer(IObjectContainer container)
        {
            Current = container;
        }


        public static void Build()
        {
            Current.Build();
        }

        public static void Register(Type serviceType, Type implementationType, LifeTime lifetime = LifeTime.Singleton)
        {
            Current.Register(serviceType, implementationType, lifetime);
        }

        public static void Register(Type serviceType, LifeTime lifetime = LifeTime.Singleton)
        {
            Current.Register(serviceType, serviceType, lifetime);
        }

        public static void Register<TService, TImplementer>(LifeTime lifetime = LifeTime.Singleton) where TImplementer : TService
        {
            Current.Register(typeof(TService), typeof(TImplementer), lifetime);
        }

        public static void Register<TService>(LifeTime lifetime = LifeTime.Singleton)
        {
            Current.Register(typeof(TService), typeof(TService), lifetime);
        }


        public static TService Resolve<TService>()
        {
            return (TService)Current.Resolve(typeof(TService));
        }

        public static object Resolve(Type serviceType)
        {
            return Current.Resolve(serviceType);
        }
    }
}
