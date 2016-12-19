using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPX_Template1.TemplateUtilities.Exceptions
{
    public class DuplicateResourceNameException : Exception
    {
        public DuplicateResourceNameException()
            : base()
        { }

        public DuplicateResourceNameException(string message)
            : base(message)
        { }

        public DuplicateResourceNameException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
