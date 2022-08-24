﻿using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ReSanctuary.Windows;

public class ConfigWindow : Window, IDisposable {
    private Configuration Configuration;

    private const ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse |
                                                 ImGuiWindowFlags.NoScrollbar |
                                                 ImGuiWindowFlags.NoScrollWithMouse;

    public ConfigWindow(Plugin plugin) : base("ReSanctuary Config", WindowFlags) {
        Size = new Vector2(300, 75);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw() {
        ImGui.Text("There's nothing to configure right now. Sorry! :P");
    }
}
