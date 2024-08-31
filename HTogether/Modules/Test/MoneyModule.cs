using HTogether.Rendering;
using HTogether.Utils;
using ImGuiNET;
using System.Drawing;

namespace HTogether.Modules;

[HModule]
public class MoneyModule() : Module("test", BaseTabID.Money)
{
	public int Amount;

	public override void RenderGUIElements()
	{
		ImGui.SliderInt("Money", ref Amount, 0, 10000);

		ImGui.SeparatorText("Host only");

		if (ImGui.Button("Add"))
		{
			GameData.Instance.NetworkgameFunds += Amount;
		}

		ImGui.SameLine();

		if (ImGui.Button("Remove"))
		{
			GameData.Instance.NetworkgameFunds -= Amount;
		}
	}

}
