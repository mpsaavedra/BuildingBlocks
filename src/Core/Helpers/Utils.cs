using System.IO;

namespace Orun.Helpers
{
    /// <summary>
    /// Some basic utils, do not know where else to put them ;D
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     ensure that a given directory exists if not create it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool EnsureDirectory(string path)
        {
            if (!Directory.Exists(path)) 
                Directory.CreateDirectory(path);

            return true;
        }
    }
}