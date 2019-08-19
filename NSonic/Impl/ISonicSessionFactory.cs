using System;

namespace NSonic.Impl
{
    interface ISonicSessionFactory : IDisposable
    {
        ISonicSession Create(EnvironmentResponse environment);
    }
}
