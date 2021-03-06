﻿using Autofac;
using Goofy.Common.Autofac;
using Goofy.Common.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Configurations
{
    public static class ConfigurationExtensions
    {
       
        public static Configuration UseAutofac(this Configuration configuration)
        {
            return UseAutofac(configuration, new ContainerBuilder());
        }
        
        public static Configuration UseAutofac(this Configuration configuration, ContainerBuilder containerBuilder)
        {
            ObjectContainer.SetContainer(new AutofacObjectContainer(containerBuilder));
            return configuration;
        }
    }
}
