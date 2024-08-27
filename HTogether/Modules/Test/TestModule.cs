using HTogether.Rendering;
using ImGuiNET;

namespace HTogether.Modules;

[CheatModule]
public class TestModule() : Module("test", TabID.TheH)
{

	public override void RenderGUIElements()
	{
		ImGui.Text("I LOVE H");
	}

}
