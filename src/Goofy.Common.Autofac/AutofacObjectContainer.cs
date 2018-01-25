using Goofy.Common.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Goofy.Common.Autofac
{
    public class AutofacObjectContainer : IObjectContainer
    {
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;


        public AutofacObjectContainer() : this(new ContainerBuilder())
        {
        }

        public AutofacObjectContainer(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }


        public ContainerBuilder ContainerBuilder
        {
            get
            {
                return _containerBuilder;
            }
        }

        public IContainer Container
        {
            get
            {
                return _container;
            }
        }

        public void Build()
        {
            _container = _containerBuilder.Build();
        }


        public void RegisterType(Type implementationType, LifeStyle life = LifeStyle.Singleton)
        {
            if (implementationType.IsGenericType)
            {
                var registrationBuilder = _containerBuilder.RegisterGeneric(implementationType);
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
            else
            {
                var registrationBuilder = _containerBuilder.RegisterType(implementationType);
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
        }

        public void RegisterType<TImplementer>(LifeStyle life = LifeStyle.Singleton)
        {
            var registrationBuilder = _containerBuilder.RegisterType<TImplementer>();
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }

        public void RegisterType(Type serviceType, Type implementationType, LifeStyle life = LifeStyle.Singleton)
        {
            if (implementationType.IsGenericType)
            {
                var registrationBuilder = _containerBuilder.RegisterGeneric(implementationType).As(serviceType);
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
            else
            {
                var registrationBuilder = _containerBuilder.RegisterType(implementationType).As(serviceType);
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
        }

        public void Register<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = _containerBuilder.RegisterType<TImplementer>().As<TService>();
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }

        public void Register<TService, TImplementer>(Func<TImplementer> fun, LifeStyle life = LifeStyle.Singleton)
           where TService : class
           where TImplementer : class, TService
        {
            var registrationBuilder = _containerBuilder.Register<TService>(x => fun());
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }





        public void RegisterInstance<TService, TImplementer>(TImplementer instance)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = _containerBuilder.RegisterInstance(instance).As<TService>().SingleInstance();
        }



        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public bool TryResolve<TService>(out TService instance) where TService : class
        {
            return _container.TryResolve(out instance);
        }

        public bool TryResolve(Type serviceType, out object instance)
        {
            return _container.TryResolve(serviceType, out instance);
        }




    }
}
