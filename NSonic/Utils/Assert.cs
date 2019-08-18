namespace NSonic.Utils
{
    static class Assert
    {
        public static void IsTrue(bool result, string exceptionMessage)
        {
            if (!result)
            {
                throw new AssertionException(exceptionMessage);
            }
        }
    }
}
