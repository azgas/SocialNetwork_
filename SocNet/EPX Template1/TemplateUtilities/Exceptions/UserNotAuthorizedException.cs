using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPX_Template1.TemplateUtilities.Exceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException()
            : base()
        { }

        public UserNotAuthorizedException(string message)
            : base(message)
        { }

        public UserNotAuthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}