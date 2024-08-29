using HTogether.Rendering;
using System.Runtime.CompilerServices;

namespace HTogether.Modules;

public class Module
{
	public string Name { get; protected set; }

	public int Tab { get; protected set; } 

	public virtual void Init()
	{
	}

	public Module(string name, int tab)
	{
		Name = name;
		Tab = tab;

		Init();
	}

	internal Module(string name, TabID tabId) : this(name, (int)tabId)
	{
		// Empty
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void RenderGUIElements() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void OnRender() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void Update() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void FixedUpdate() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void OnGUI() { }

}
