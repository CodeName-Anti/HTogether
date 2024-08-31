using HTogether.Rendering;
using HTogether.Utils;
using ImGuiNET;
using System.Drawing;

namespace HTogether.Modules;

[HModule]
public class MoneyModule() : Module("test", BaseTabID.Money)
{
	private int amount;

	public override void RenderGUIElements()
	{
		ImGui.TextColored(Color.Red.ToSysVec(), "Host only");

		ImGui.SliderInt("Money", ref amount, 0, 10000);

		if (ImGui.Button("Add"))
		{
			GameData.Instance.NetworkgameFunds += amount;
		}

		ImGui.SameLine();

		if (ImGui.Button("Remove"))
		{
			GameData.Instance.NetworkgameFunds -= amount;
		}
	}

}
