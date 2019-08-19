using NSonic.Utils;
using System;
using System.Diagnostics;

namespace NSonic.Impl.Connections
{
    abstract class SonicConnection : ISonicConnection
    {
        private readonly ISonicSessionFactoryProvider sessionFactoryProvider;
        private readonly string hostname;
        private readonly int port;
        private readonly string secret;

        protected SonicConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , ISonicRequestWriter requestWriter
            , string hostname
            , int port
            , string secret
            )
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.RequestWriter = requestWriter;
            this.hostname = hostname;
            this.port = port;
            this.secret = secret;
        }

        protected abstract string Mode { get; }

        protected ISonicRequestWriter RequestWriter { get; }
        protected ISonicSessionFactory SessionFactory { get; private set; }

        public void Connect()
        {
            this.SessionFactory = this.sessionFactoryProvider.Create(hostname, port);

            using (var session = this.SessionFactory.Create())
            {
                var response = session.Read();
                Assert.IsTrue(response.StartsWith("CONNECTED"), "Did not receive connection confirmation from the server");

                session.Write("START", this.Mode, this.secret);

                response = session.Read();
                Assert.IsTrue(response.StartsWith("STARTED"), "Failed to start control session");

                // TODO: Handle STARTED properly.
            }
        }

        public void Dispose()
        {
            if (this.SessionFactory != null)
            {
                using (var session = this.SessionFactory.Create())
                {
                    try
                    {
                        session.Write("QUIT");
                        Assert.IsTrue(session.Read().StartsWith("ENDED"), "Quit failed when disposing sonic connection");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }

                this.SessionFactory.Dispose();
            }
        }
    }
}
