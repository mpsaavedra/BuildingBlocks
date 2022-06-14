using System;

namespace Orun.Helpers
{
    /// <summary>
    /// A very simple helper class to generate random strings
    /// </summary>
    public class RandomGenerator
    {
        private static Random _rand = new Random();

        /// <summary>
        /// generates a new random string of a given lenght
        /// </summary>
        /// <param name="lenght"></param>
        /// <param name="codeBase"></param>
        /// <returns></returns>
        public static string NewRandomString(int lenght, string codeBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            _rand ??= new Random();

            var result = "";
            for (var i = 0; i < lenght; i++)
            {
                var index = _rand.Next(codeBase.Length);
                result += codeBase[index];
            }

            return result;
        }
    }
}