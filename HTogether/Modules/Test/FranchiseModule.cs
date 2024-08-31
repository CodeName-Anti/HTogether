using HTogether.Rendering;
using ImGuiNET;

namespace HTogether.Modules.Test;

[HModule]
public class FranchiseModule() : Module("Franchise", BaseTabID.Franchise)
{

	private int amount;

	public override void RenderGUIElements()
	{
		ImGui.SliderInt("Amount", ref amount, 0, 150);

		if (ImGui.Button("Add exp"))
		{
			GameData.Instance.NetworkgameFranchiseExperience += amount;
		}

		ImGui.SameLine();

		if (ImGui.Button("Remove exp"))
		{
			GameData.Instance.NetworkgameFranchiseExperience -= amount;
		}

		if (ImGui.Button("Add points"))
		{
			GameData.Instance.NetworkgameFranchisePoints += amount;
		}

		ImGui.SameLine();

		if (ImGui.Button("Remove Points"))
		{
			GameData.Instance.NetworkgameFranchisePoints -= amount;
		}
	}

}
