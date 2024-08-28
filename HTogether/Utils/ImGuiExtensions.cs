using ImGuiNET;
using UnityEngine;

using SysVec2 = System.Numerics.Vector2;
using SysVec3 = System.Numerics.Vector3;
using SysVec4 = System.Numerics.Vector4;

using SysColor = System.Drawing.Color;

namespace HTogether.Utils;

public static class ImGuiExtensions
{

	public static SysVec2 ToSysVec(this Vector2 vector)
	{
		return new SysVec2(vector.x, vector.y);
	}

	public static SysVec3 ToSysVec(this Vector3 vector)
	{
		return new SysVec3(vector.x , vector.y, vector.z);
	}

	public static SysVec2 ToSysVec2(this Vector3 vector)
	{
		return new SysVec2(vector.x, vector.y);
	}

	public static SysVec4 ToSysVec(this Color color)
	{
		return new SysVec4(color.r, color.g, color.b, color.a);
	}

	public static SysVec4 ToSysVec(this SysColor color)
	{
		return new SysVec4(color.R / 255, color.G / 255, color.B / 255, color.A / 255);
	}

	public static uint ToImguiColor(this SysVec4 vector)
	{
		return ImGui.ColorConvertFloat4ToU32(vector);
	}

	public static uint ToImGuiColor(this Color color)
	{
		return color.ToSysVec().ToImguiColor();
	}

}
