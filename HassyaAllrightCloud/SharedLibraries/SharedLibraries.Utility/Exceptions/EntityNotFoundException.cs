using System;

namespace SharedLibraries.Utility.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string error)
        : base(error)
        {

        }
    }
}
