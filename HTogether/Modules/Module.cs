using HTogether.Rendering;
using System.Runtime.CompilerServices;

namespace HTogether.Modules;

public class Module
{
    public string Name { get; protected set; }

    public TabID Tab { get; protected set; } 

    public virtual void Init(bool json = false)
    {
    }

    public Module(string name, TabID tab)
    {
        Name = name;
        Tab = tab;

        Init();
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
