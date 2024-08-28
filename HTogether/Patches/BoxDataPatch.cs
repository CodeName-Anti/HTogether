using HarmonyLib;
using UnityEngine;

namespace HTogether.Patches;

[HarmonyPatch(typeof(BoxData))]
public static class BoxDataPatch
{
	public static bool DisableBoxCollisions;

	public static int InitialLayer = -1;

	[HarmonyPatch(nameof(BoxData.SetBoxData))]
	[HarmonyPostfix]
	public static void SetBoxData(BoxData __instance)
	{
		if (!DisableBoxCollisions)
			return;

		if (InitialLayer == -1)
			InitialLayer = __instance.gameObject.layer;

		__instance.gameObject.layer = 20;

		Physics.IgnoreLayerCollision(20, 20);
	}

}
