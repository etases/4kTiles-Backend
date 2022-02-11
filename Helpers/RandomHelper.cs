namespace _4kTiles_Backend.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random RandomGenerator = new();

        /// <summary>
        /// Generate a random string from the seed
        /// </summary>
        /// <param name="seed">the seed</param>
        /// <param name="length">the length of the final string</param>
        /// <returns>the random string</returns>
        public static string Random(this string seed, int length)
        {
            var stringChars = new char[length];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = seed[RandomGenerator.Next(seed.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Generate a random string of number
        /// </summary>
        /// <param name="length">the length of the final string</param>
        /// <returns>the random string</returns>
        public static string RandomNumber(int length) => Random("0123456789", length);
    }
}
