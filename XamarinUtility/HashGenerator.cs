namespace XamarinUtility
{
    public static class HashGenerator
    {
        // Taken from https://stackoverflow.com/a/5155015/2304737
        public static int PersistentHashInt(string text)
        {
            unchecked
            {
                int hash = 23;
                foreach (char c in text)
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }

        public static long PersistentHashLong(string text)
        {
            unchecked
            {
                long hash = 23;
                foreach (char c in text)
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }

        public static string PersistentHashString(string text)
        {
            return PersistentHashLong(text).ToString();
        }
    }
}
