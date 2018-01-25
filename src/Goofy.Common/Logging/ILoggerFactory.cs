using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Logging
{
    public interface ILoggerFactory
    {
        ILogger Create(string name);

        ILogger Create(Type type);
    }
}
