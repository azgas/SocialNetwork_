using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPXSocNet.TemplateUtilities.Exceptions
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
