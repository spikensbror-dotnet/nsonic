using Moq;
using NSonic.Impl;
using System.Threading.Tasks;

namespace NSonic.Tests.Stubs
{
    static class FluentISonicSessionMockSetupExtensions
    {
        public static Mock<ISonicSession> SetupRead(this Mock<ISonicSession> mock
            , MockSequence sequence
            , bool async
            , string result
            )
        {
            if (async)
            {
                mock
                    .InSequence(sequence)
                    .Setup(s => s.ReadAsync())
                    .Returns(Task.FromResult(result))
                    ;
            }
            else
            {
                mock
                    .InSequence(sequence)
                    .Setup(s => s.Read())
                    .Returns(result)
                    ;
            }

            return mock;
        }

        public static Mock<ISonicSession> SetupWrite(this Mock<ISonicSession> mock
            , MockSequence sequence
            , bool async
            , params string[] args
            )
        {
            if (async)
            {
                mock
                    .InSequence(sequence)
                    .Setup(s => s.WriteAsync(args))
                    .Returns(Task.CompletedTask)
                    ;
            }
            else
            {
                mock
                    .InSequence(sequence)
                    .Setup(s => s.Write(args))
                    ;
            }

            return mock;
        }

        public static Mock<ISonicSession> SetupWriteWithResult(this Mock<ISonicSession> mock
            , MockSequence sequence
            , bool async
            , string result
            , params string[] args
            )
        {
            return mock
                .SetupWrite(sequence, async, args)
                .SetupRead(sequence, async, $"RESULT {result}")
                ;
        }

        public static Mock<ISonicSession> SetupWriteWithOk(this Mock<ISonicSession> mock
            , MockSequence sequence
            , bool async
            , params string[] args
            )
        {
            return mock
                .SetupWrite(sequence, async, args)
                .SetupRead(sequence, async, "OK")
                ;
        }
    }
}
