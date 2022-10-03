﻿using System.IO;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using ReSanctuary.Windows;
using ImGuiScene;
using System.Collections.Generic;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using System.Linq;

namespace ReSanctuary;

public sealed class Plugin : IDalamudPlugin {
    public string Name => "ReSanctuary";
    private const string CommandName = "/psanctuary";

    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] public static CommandManager CommandManager { get; private set; }
    [PluginService] public static DataManager DataManager { get; private set; }
    [PluginService] public static SigScanner SigScanner { get; private set; }

    public Configuration Configuration { get; private set; }
    public WindowSystem WindowSystem = new("ReSanctuary");

    public static Dictionary<uint, TextureWrap> IconCache = new();
    public static TerritoryType IslandSanctuary { get; set; }

    public Plugin() {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        WindowSystem.AddWindow(new ConfigWindow(this));
        WindowSystem.AddWindow(new MainWindow(this));
        WindowSystem.AddWindow(new WidgetWindow(this));

        ExcelSheet<TerritoryType> territoryTypeSheet = DataManager.Excel.GetSheet<TerritoryType>();
        IslandSanctuary = territoryTypeSheet.First(x => x.Name == "h1m2");

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) {
            HelpMessage = "Opens the main ReSanctuary interface."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose() {
        WindowSystem.RemoveAllWindows();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args) {
        switch (args) {
            case "settings":
            case "config":
                DrawConfigUI();
                break;
            case "widget":
                WindowSystem.GetWindow("RS Todo List").IsOpen ^= true;
                break;
            default:
                WindowSystem.GetWindow("ReSanctuary").IsOpen ^= true;
                break;
        }
    }

    private void DrawUI() {
        WindowSystem.Draw();
    }

    public void DrawConfigUI() {
        WindowSystem.GetWindow("ReSanctuary Config").IsOpen = true;
    }
}
