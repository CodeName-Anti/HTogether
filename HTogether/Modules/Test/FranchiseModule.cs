using HTogether.Rendering;
using ImGuiNET;

namespace HTogether.Modules.Test;

[CheatModule]
public class FranchiseModule() : Module("Franchise", TabID.Franchise)
{

	private int amount;

	public override void RenderGUIElements()
	{
		ImGui.SliderInt("Amount", ref amount, 0, 10000);

		if (ImGui.Button("Add exp"))
		{
			GameData.Instance.NetworkgameFranchiseExperience += 6969;
		}

		if (ImGui.Button("Add points"))
		{
			GameData.Instance.NetworkgameFranchisePoints += 6969;
		}
	}

}
