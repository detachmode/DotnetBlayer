using System;
using System.Linq;
using System.Reflection;
namespace DotnetBlayer
{
    public class BlaToolHelper
    {
        public static Type[] GetFromExampleDll()
        {
            Type[] iLoadTypes = (from t in typeof(BlaToolHelper).Assembly.GetExportedTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(IBlayerTool).IsAssignableFrom(t)
                                 select t).ToArray();

            return iLoadTypes;
        }
    }

}