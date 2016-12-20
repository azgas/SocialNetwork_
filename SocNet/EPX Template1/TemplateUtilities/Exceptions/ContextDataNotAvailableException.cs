﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPX_Template1.TemplateUtilities.Exceptions
{
    public class ContextDataNotAvailableException : Exception
    {
        public ContextDataNotAvailableException()
            : base()
        { }

        public ContextDataNotAvailableException(string message)
            : base(message)
        { }

        public ContextDataNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}