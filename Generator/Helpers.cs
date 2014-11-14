namespace Generator
{
    using System.Reflection;

    internal static class Helpers
    {
        internal static string GetRootPath()
        {
            string rootPath = Assembly.GetExecutingAssembly().Location;

#if DEBUG
            {
                return rootPath.Replace("Generator\\bin\\Debug\\Generator.exe", string.Empty);
            }
#else
            {
                return rootPath.Replace("Generator\\bin\\Release\\Generator.exe", string.Empty);
            }
#endif

        }
    }
}
