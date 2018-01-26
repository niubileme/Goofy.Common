using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common
{
    public interface IObjectContainer
    {

        void Build();


        void RegisterType(Type implementationType, LifeStyle life = LifeStyle.Singleton);

        void RegisterType<TImplementer>(LifeStyle life = LifeStyle.Singleton);



        void RegisterType(Type serviceType, Type implementationType, LifeStyle life = LifeStyle.Singleton);

        void Register<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
           where TService : class
           where TImplementer : class, TService;



        void RegisterInstance<TService, TImplementer>(TImplementer instance)
           where TService : class
           where TImplementer : class, TService;



        TService Resolve<TService>() where TService : class;

        bool TryResolve<TService>(out TService instance) where TService : class;

        object Resolve(Type serviceType);

        bool TryResolve(Type serviceType, out object instance);


    }
}
