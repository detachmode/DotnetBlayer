using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace DotnetBlayer
{
    public class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext()
           : base(isCollectible: true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName) => null;
    }

    public class BlaToolHelper
    {
        public static SimpleUnloadableAssemblyLoadContext context;
        private static WeakReference alcWeakRef;

        public static IBlayerTool[] GetFromExampleDll()
        {
            try
            {
                GC.Collect();
                var assemblyName = @"/Users/dennismuller/DotnetBlayer/ExampleApp/bin/Debug/netcoreapp3.0/ExampleApp.dll";

            if (context != null)
            {
                context.Unload();
                // for (int i = 0; alcWeakRef.IsAlive && (i < 10); i++)
                // {
                //     System.Console.WriteLine("GC caöllll");
                //     GC.Collect();
                //     GC.WaitForPendingFinalizers();
                // }
            }
            
            context = new SimpleUnloadableAssemblyLoadContext();
            alcWeakRef = new WeakReference(context, trackResurrection: true);
            Assembly assembly = context.LoadFromAssemblyPath(assemblyName);



            Type[] iLoadTypes = (from t in assembly.GetExportedTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(IBlayerTool).IsAssignableFrom(t)
                                 select t).ToArray();

            return iLoadTypes.Select(t => (IBlayerTool)Activator.CreateInstance(t)).ToArray();

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return null;
            }

    
        }
    }

}