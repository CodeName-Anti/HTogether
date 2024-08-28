using HarmonyLib;
using HTogether.Patches;
using HTogether.Rendering;
using HTogether.Utils;
using ImGuiNET;
using UnityEngine;

namespace HTogether.Modules.Test;

[CheatModule]
public class TestingModule() : Module("Testing", TabID.Testing)
{

	private int amount = 0;

	public override void RenderGUIElements()
	{
		ImGui.SliderFloat("Box spawner delay", ref ManagerBlackboardPatch.SpawnDelay, 0, 2);

		if (ImGui.Checkbox("Disable Box Collisions", ref BoxDataPatch.DisableBoxCollisions) && !BoxDataPatch.DisableBoxCollisions)
		{
			GameObject.FindObjectsByType<BoxData>(FindObjectsSortMode.None).Do(b => b.gameObject.layer = BoxDataPatch.InitialLayer);
		}

		ImGui.TextColored(Color.red.ToSysVec(), "Look directly at box without escape menu open");
		ImGui.TextColored(Color.red.ToSysVec(), "while being far enough away to not pick it up");

		ImGui.SliderInt("Box Item Amount", ref amount, 0, 10000);

		ImGui.SameLine();

		if (ImGui.Button("Set amount"))
		{
			Camera main = Camera.main;
			if (!Physics.Raycast(main.transform.position, main.transform.forward, out RaycastHit hitInfo))
				return;

			GameObject obj = hitInfo.transform.gameObject;

			BoxData data = obj.GetComponentInParent<BoxData>();

			if (data == null)
				return;

			data.numberOfProducts = amount;
		}
	}

}
