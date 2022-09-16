using System;

namespace SharedLibraries.Utility.Exceptions
{
    public class EncryptException : Exception
    {
        public EncryptException() { }
        public EncryptException(string error)
        : base(error)
        {

        }
    }
}
