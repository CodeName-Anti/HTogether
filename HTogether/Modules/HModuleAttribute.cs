using System;
using System.Linq;
using System.Reflection;

namespace HTogether.Modules;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class HModuleAttribute : Attribute
{
    public static Type[] GetAllModuleTypes()
    {
        return GetAllModulesTypes(Assembly.GetExecutingAssembly());
    }

    public static Type[] GetAllModulesTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(m => m.GetCustomAttributes(typeof(HModuleAttribute), false).Length > 0 && m.IsSubclassOf(typeof(Module)))
            .ToArray();
    }

}
