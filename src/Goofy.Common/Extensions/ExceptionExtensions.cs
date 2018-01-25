using System;
using System.Collections.Generic;
using System.Text;

namespace Goofy.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetAllInnerMessages(this Exception ex)
        {
            return ex.InnerException != null ? $"{ex.Message}[{GetAllInnerMessages(ex.InnerException)}]" : ex.Message;
        }
    }
}
