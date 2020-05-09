using NSonic.Impl;

namespace NSonic.Tests.Stubs
{
    static class StubConstants
    {
        public const string Hostname = "localhost";
        public const int Port = 14966;
        public const string Secret = "ThisIsASecret";

        // Buffer is 20002 because we need to differentiate from Environment.Default
        public static readonly EnvironmentResponse ConnectedEnvironment = new EnvironmentResponse(1, 20002);
    }
}
