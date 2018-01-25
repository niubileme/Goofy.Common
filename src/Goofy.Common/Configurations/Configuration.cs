using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Configurations
{
    public class Configuration
    {
        public static Configuration Instance { get; private set; }

        private Configuration() { }

        public static Configuration Create()
        {
            Instance = new Configuration();
            return Instance;
        }
    }
}
