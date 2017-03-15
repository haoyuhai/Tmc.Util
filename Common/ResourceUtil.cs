using System.IO;
using System.Reflection;
using System.Text;

namespace Tmc.Util
{
    public static class ResourceUtil
    {
        public static string GetResourceText(Assembly assembly, string resourceName)
        {
            using (StreamReader streamReader = new StreamReader(assembly.GetManifestResourceStream(resourceName), Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
