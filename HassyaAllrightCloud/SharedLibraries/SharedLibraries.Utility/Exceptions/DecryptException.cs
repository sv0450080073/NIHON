using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.Utility.Exceptions
{
    public class DecryptException : Exception
    {
        public DecryptException() { }
        public DecryptException(string error)
        : base(error)
        {

        }
    }
}
