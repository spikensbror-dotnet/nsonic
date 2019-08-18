using System;

namespace NSonic
{
    public class AssertionException : Exception
    {
        internal AssertionException(string message)
            : base(message)
        {
            //
        }
    }
}
