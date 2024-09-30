using HarmonyLib;
using HTogether.Patches;
using HTogether.Rendering;
using HTogether.Utils;
using ImGuiNET;
using UnityEngine;

namespace HTogether.Modules.Test;

[HModule]
public class TestingModule() : Module("Testing", BaseTabID.Testing)
{
	private int amount = 0;

	private float setItemAmountRadius = 1.8f;

	public override void RenderGUIElements()
	{
		ImGui.SliderFloat("Box spawner delay", ref ManagerBlackboardPatch.SpawnDelay, 0, 2);

		if (ImGui.Checkbox("Disable Box Collisions", ref BoxDataPatch.DisableBoxCollisions) && !BoxDataPatch.DisableBoxCollisions)
		{
			GameObject.FindObjectsByType<BoxData>(FindObjectsSortMode.None).Do(b => b.gameObject.layer = BoxDataPatch.InitialLayer);
		}

		ImGui.SliderInt("Box Item Amount", ref amount, 0, 10000);

		ImGui.SliderFloat("Range", ref setItemAmountRadius, 0, 30);

		if (ImGui.Button("Set amount"))
		{
			SetBoxAmount();
		}

		ImGui.SameLine();
		ImGuiExtensions.HelpMarker("Sets the item amount of the boxes in a small radius.");
	}

	private void SetBoxAmount()
	{
		if (!LobbyController.Instance.LocalplayerController.isServer && HTogether.LockdownFeatures)
			return;

		Collider[] colliders = Physics.OverlapSphere(LobbyController.Instance.LocalplayerController.transform.position, setItemAmountRadius);

		foreach (Collider collider in colliders)
		{
			if (!collider.TryGetComponent<BoxData>(out BoxData data))
				continue;

			data.numberOfProducts = amount;
		}
	}

}
