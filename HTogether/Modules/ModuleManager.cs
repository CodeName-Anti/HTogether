using System;
using System.Collections.Generic;
using System.Linq;

namespace HTogether.Modules;

public class ModuleManager
{
    public static ModuleManager Instance { get; private set; }

    public List<Module> Modules { get; private set; }

    public void ExecuteForModules(Action<Module> action)
    {
        foreach (Module module in Modules)
        {
            try
            {
                action(module);
            }
            catch (Exception ex)
            {
                HTogether.Logger.LogError("Exception in Module \"" + module.Name + "\": " + ex.ToString());
            }
        }
    }

    public T GetModule<T>() where T : Module
    {
        return Modules.OfType<T>().FirstOrDefault();
    }

    public ModuleManager()
    {
        Instance = this;
        Modules = new(CheatModuleAttribute.InstantiateAll());
	}
}
