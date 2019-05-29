using System;
using System.Linq;
using System.Reflection;
namespace DotnetBlayer
{
    public class BlaToolHelper
    {
        public static IBlayerTool[] GetFromExampleDll()
        {
            Type[] iLoadTypes = (from t in typeof(BlaFileListing).Assembly.GetExportedTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(IBlayerTool).IsAssignableFrom(t)
                                 select t).ToArray();

            return iLoadTypes.Select(t => (IBlayerTool)Activator.CreateInstance(t)).ToArray();

        }
    }

}