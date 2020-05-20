namespace NSonic.Utils
{
    static class Assert
    {
        public static void IsTrue(bool result, string exceptionMessage, string actual)
        {
            if (!result)
            {
                throw new AssertionException(exceptionMessage, actual);
            }
        }
    }
}
