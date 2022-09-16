using System;

namespace SharedLibraries.Utility.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() { }
        public DuplicateException(string error)
        : base(error)
        {

        }
    }
}
