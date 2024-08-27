using ImGuiNET;
using HTogether.Rendering;

namespace HTogether.Modules;

public class ToggleModule : Module
{
	public bool Enabled
	{
		get => enabled;
		protected set => enabled = value;
	}

	protected bool enabled;

	public ToggleModule(string name, TabID tab) : base(name, tab) { }

	public override void RenderGUIElements()
	{
		base.RenderGUIElements();

		if (ImGui.Checkbox(Name, ref enabled))
		{
			OnToggle();
		}
	}

	protected virtual void OnToggle() { }

}
