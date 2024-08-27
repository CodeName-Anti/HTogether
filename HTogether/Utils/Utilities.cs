using System;
using System.IO;
using System.Reflection;

namespace HTogether.Utils;

public static class Utilities
{
    public static string Format(this Type[] types)
    {
        // Return Empty string if no types are in array
        if (types.Length == 0)
            return string.Empty;

        string formatted = "";

        foreach (var type in types)
        {
            formatted += type.FullName + ", ";
        }

        // Remove last 2 characters
        return formatted.Substring(0, formatted.Length - 2);
    }

    /// <summary>
    /// Formats the version of an Assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="pretty"></param>
    /// <returns></returns>
    public static string FormatAssemblyVersion(Assembly assembly, bool pretty = false)
    {
        if (assembly == null)
            assembly = Assembly.GetExecutingAssembly();

        string version = assembly.GetName().Version.ToString();
        for (int i = 0; i < 100; i++)
        {
            if (version.EndsWith(".0"))
                version = version.Trim().Substring(0, version.Length - 2);
            else
                break;
        }

        // Read a single ".0" to make it look nicer
        if (pretty && !version.Contains("."))
        {
            version += ".0";
        }

        return version;
    }

    public static string GetAssemblyLocation()
    {
        return GetAssemblyLocation(Assembly.GetExecutingAssembly());
    }

    public static string GetAssemblyLocation(Assembly assembly)
    {
        return Path.GetDirectoryName(assembly.Location);
    }

}
