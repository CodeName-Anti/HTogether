using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace HTogether.Patches;

[HarmonyPatch(typeof(ManagerBlackboard))]
public static class ManagerBlackboardPatch
{
	public static float SpawnDelay = 0.5f;

public static CodeMatcher SearchInstruction(this CodeMatcher matcher, Predicate<CodeInstruction> predicate)
{
	matcher.Start();
	matcher.Advance(matcher.Instructions().FindIndex(predicate));
	return matcher;
}

	[HarmonyPatch(nameof(ManagerBlackboard.ServerCargoSpawner), MethodType.Enumerator)]
	[HarmonyTranspiler]
	public static IEnumerable<CodeInstruction> TranspileServerCargoSpawner(IEnumerable<CodeInstruction> instructions)
	{
		return new CodeMatcher(instructions)
			.MatchForward(false,
				new CodeMatch(OpCodes.Ldarg_0),
				new CodeMatch(OpCodes.Ldc_R4, 0.5f),
				new CodeMatch(OpCodes.Newobj),
				new CodeMatch(OpCodes.Stfld))
			.Advance(1)
			.RemoveInstruction()
			.Insert(new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(ManagerBlackboardPatch), "SpawnDelay")))
			.InstructionEnumeration();
	}

}
