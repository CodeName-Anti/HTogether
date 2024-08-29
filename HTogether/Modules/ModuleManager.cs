using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTogether.Modules;

public class ModuleManager
{
	public static ModuleManager Instance { get; private set; }

	public List<Module> Modules { get; private set; } = [];

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

	public void RegisterModules(IEnumerable<Module> modules)
	{
		Modules.AddRange(modules);
	}

	public void RegisterModules(Assembly assembly)
	{
		RegisterModules(HModuleAttribute
			.GetAllModulesTypes(assembly)
			.Select(t => Activator.CreateInstance(t, null) as Module));
	}

	public void RegisterModule(Module module)
	{
		Modules.Add(module);
	}

	public T GetModule<T>() where T : Module
	{
		return Modules.OfType<T>().FirstOrDefault();
	}

	public ModuleManager()
	{
		Instance = this;
		RegisterModules(Assembly.GetExecutingAssembly());
	}
}
