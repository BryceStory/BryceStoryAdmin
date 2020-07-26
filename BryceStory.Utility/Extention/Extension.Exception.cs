using System;
using System.Collections.Generic;
using System.Text;

namespace BryceStory.Utility.Extention
{
    public static partial class Extension
    {
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetOriginalException();
        }
    }
}
