using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HTogether.Modules;
using HTogether.Rendering;
using HTogether.Utils;
using System;
using System.Runtime.CompilerServices;

namespace HTogether;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class HTogether : BaseUnityPlugin
{
	public static HTogether Instance { get; private set; }

	public static bool LockdownFeatures { get; set; } = true;

	public static new ManualLogSource Logger { get; private set; }
	public Harmony HarmonyInstance { get; private set; }

	public bool Shown => Renderer != null && Renderer.RenderGUI;

	public ModuleManager ModuleManager;

	public GUIRenderer Renderer { get; private set; }

	private void Awake()
	{
		Instance = this;
		Logger = base.Logger;

		gameObject.AddComponent<UnityMainThreadDispatcher>();

		// Initialize Harmony
		try
		{
			HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);

			HarmonyInstance.PatchAll();
		}
		catch (Exception ex)
		{
			Logger.LogError("Harmony patching error: " + ex.ToString());
		}

		ModuleManager = new ModuleManager();

		Renderer = new();

		Renderer.Initialize();
	}

	private void OnApplicationQuit()
	{
		Renderer.Shutdown();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Update()
	{
		// Run Update on every module
		ModuleManager.ExecuteForModules(m => m.Update());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void FixedUpdate()
	{
		// Run FixedUpdate on every module
		ModuleManager.ExecuteForModules(m => m.FixedUpdate());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnGUI()
	{
		// Run OnGUI on every Module
		ModuleManager.ExecuteForModules(m => m.OnGUI());
	}
}
