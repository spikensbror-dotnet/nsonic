using Moq;
using NSonic.Impl;
using System.Threading.Tasks;

namespace NSonic.Tests.Stubs
{
    static class FluentISessionMockSetupExtensions
    {
        public static Mock<ISession> SetupRead(this Mock<ISession> mock
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

        public static Mock<ISession> SetupWrite(this Mock<ISession> mock
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

        public static Mock<ISession> SetupWriteWithResult(this Mock<ISession> mock
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

        public static Mock<ISession> SetupWriteWithOk(this Mock<ISession> mock
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
