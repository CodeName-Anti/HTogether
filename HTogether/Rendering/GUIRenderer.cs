﻿using HarmonyLib;
using HTogether.Utils;
using IconFonts;
using ImGuiNET;
using SharpGUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace HTogether.Rendering;

public class GUIRenderer
{
	public SortedDictionary<int, GUITab> Tabs { get; private set; }
	public int CurrentTabId { get; private set; }

	public event Action OnDrawIntro = delegate { };

	public ImGuiKey MenuKey = ImGuiKey.RightShift;

	public bool RenderGUI
	{
		get => _RenderGUI;
		set
		{
			_RenderGUI = value;
			GUI.HandleInput = _RenderGUI;
			GUI.BlockInput = _RenderGUI;
		}
	}
	private bool _RenderGUI;

	private bool Intro = true;
	private bool updateAvailable;

	public void Initialize()
	{
		Tabs = [];

		try
		{
			updateAvailable = UpdateChecker.IsUpdateAvailable("CodeName-Anti", "HTogether", MyPluginInfo.PLUGIN_VERSION);
		} catch(Exception ex)
		{
			HTogether.Logger.LogError($"Error fetching GitHub releases: {ex}");
		}

		foreach (BaseTabID tabIdEnum in Enum.GetValues(typeof(BaseTabID)))
		{
			int tabId = (int)tabIdEnum;

			Tabs.Add(tabId, new GUITab(Enum.GetName(typeof(BaseTabID), tabIdEnum), tabId));
		}

		GUI.OnRender += OnRender;
		GUI.OnInitImGui += InitImGui;
		GUI.Initialize();

		GUI.HandleInput = true;
		GUI.BlockInput = false;
	}


	public void Shutdown()
	{
		GUI.Shutdown();
	}

	public int CreateTab(string name)
	{
		GUITab tab = new(name, Tabs.Keys.Last() + 1);
		Tabs.Add(tab.Id, tab);
		return tab.Id;
	}
	
	private unsafe void InitImGui()
	{
		try
		{
			ImGuiStyles.SetupFutureDarkStyle();

			string assetsPath = typeof(HTogether).Namespace + ".Assets";
			Assembly assembly = Assembly.GetExecutingAssembly();

			ImFontAtlasPtr fonts = ImGui.GetIO().Fonts;

			float baseFontSize = 15f;
			float iconFontSize = baseFontSize / 3 * 3.5f;

			ImFontPtr robotoFont = fonts.LoadFontFromResources(assetsPath + ".Roboto.Roboto-Regular.ttf", assembly, baseFontSize);
			ImGui.GetIO().NativePtr->FontDefault = robotoFont.NativePtr;

			(uint, uint) fontAwesomeRange = (FontAwesome6.IconMin, FontAwesome6.IconMax16);
			fonts.LoadIconFontFromResources(assetsPath + ".FontAwesome." + FontAwesome6.FontIconFileNameFAR, assembly, iconFontSize, fontAwesomeRange);
			fonts.LoadIconFontFromResources(assetsPath + ".FontAwesome." + FontAwesome6.FontIconFileNameFAS, assembly, iconFontSize, fontAwesomeRange);

			fonts.Build();

		} catch (Exception ex)
		{
			HTogether.Logger.LogError($"Failed to init ImGui {ex}");
		}
	}

	public void DrawUpdateAvailable(string name, string url)
	{
		ImGui.Separator();

		ImGui.PushStyleColor(ImGuiCol.Text, Color.Red.ToSysVec());
		ImGui.PushStyleColor(ImGuiCol.TextLink, Color.Red.ToSysVec());

		ImGui.Text($"An update for {name} is available, visit:");
		ImGui.TextLinkOpenURL(url);

		ImGui.PopStyleColor(2);

		ImGui.Separator();
	}

	private void RenderIntro()
	{
		if (!ImGui.Begin($"HTogether {MyPluginInfo.PLUGIN_VERSION}", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoResize  | ImGuiWindowFlags.NoMove))
		{
			ImGui.End();
			return;
		}

		Vector2 displaySize = ImGui.GetIO().DisplaySize;
		Vector2 windowSize = ImGui.GetWindowSize();
		Vector2 windowPos = new((displaySize.X - windowSize.X) / 2, (displaySize.Y - windowSize.Y) / 2);

		ImGui.SetWindowPos(windowPos);
		ImGui.Text($"Welcome to HTogether {MyPluginInfo.PLUGIN_VERSION}!");

		if (updateAvailable)
		{
			DrawUpdateAvailable("HTogether", "https://github.com/CodeName-Anti/HTogether");
		}

		OnDrawIntro();

		ImGui.Text($"To open HTogether press Right Shift.");
		ImGui.TextLinkOpenURL("Made by JNNJ", "https://github.com/CodeName-Anti/");

		ImGui.SetWindowSize(Vector2.Zero);
		ImGui.End();
	}

	private void OnRender()
	{
		try
		{
			if (ImGui.IsKeyPressed(MenuKey, false))
			{
				RenderGUI = !RenderGUI;

				if (Intro)
					Intro = false;
			}

			if (Intro)
			{
				RenderIntro();
				return;
			}

			RenderModules();
		}
		catch (Exception ex)
		{
			HTogether.Logger.LogError(ex.ToString());
		}
	}

	private void RenderModules()
	{
		// Call OnRender for things like ESP
		HTogether.Instance.ModuleManager.ExecuteForModules(m => m.OnRender());

		if (!RenderGUI)
			return;

		ImGui.Begin("HTogether by JNNJ");

		if (ImGui.BeginTabBar("tabs"))
		{
			foreach (KeyValuePair<int, GUITab> entry in Tabs)
			{
				if (!entry.Value.Enabled)
					continue;

				if (ImGui.TabItemButton(entry.Value.Name))
					CurrentTabId = entry.Key;
			}
			ImGui.EndTabBar();
		}

		HTogether.Instance.ModuleManager.Modules.Where(m => m.Tab == CurrentTabId).Do(m =>
		{
			try
			{
				m.RenderGUIElements();
			}
			catch (Exception ex)
			{
				HTogether.Logger.LogError("Exception in Module \"" + m.Name + "\": " + ex.ToString());
			}
		});

		ImGui.SetWindowSize(new Vector2(0, 0), ImGuiCond.Once);

		ImGui.End();
	}
}