﻿namespace DaLion.Stardew.Professions;

#region using directives

using System;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;

using Framework;
using Framework.AssetEditors;
using Framework.AssetLoaders;

#endregion using directives

/// <summary>The mod entry point.</summary>
public class ModEntry : Mod
{
    internal static ModConfig Config { get; set; }
    internal static PerScreen<PlayerState> PlayerState { get; private set; }
    internal static HostState HostState { get; private set; }

    internal static IModHelper ModHelper { get; private set; }
    internal static IManifest Manifest { get; private set; }
    internal static Action<string, LogLevel> Log { get; private set; }

    internal static FrameRateCounter FpsCounter { get; private set; }
    internal static ICursorPosition DebugCursorPosition { get; set; }

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        // store references to helper, mod manifest and logger
        ModHelper = helper;
        Manifest = ModManifest;
        Log = Monitor.Log;

        // get configs
        Config = helper.ReadConfig<ModConfig>();

        // initialize mod state
        PlayerState = new(() => new());
        if (Context.IsMainPlayer) HostState = new();

        // initialize mod events
        EventManager.Init(Helper.Events);

        // apply harmony patches
        PatchManager.ApplyAll(Manifest.UniqueID);

        // register asset editors / loaders
        helper.Content.AssetEditors.Add(new FishPondDataEditor());
        helper.Content.AssetEditors.Add(new SpriteEditor());
        helper.Content.AssetLoaders.Add(new TextureLoader());

        // load sound effects
        SoundBank.LoadCollection(helper.DirectoryPath);

        // add debug commands
        ConsoleCommands.Register(helper.ConsoleCommands);

        if (Context.IsMultiplayer && !Context.IsMainPlayer && !Context.IsSplitScreen)
        {
            var host = helper.Multiplayer.GetConnectedPlayer(Game1.MasterPlayer.UniqueMultiplayerID);
            var hostMod = host.GetMod(ModManifest.UniqueID);
            if (hostMod is null)
                Log("[Entry] The session host does not have this mod installed. Some features will not work properly.",
                    LogLevel.Warn);
            else if (!hostMod.Version.Equals(ModManifest.Version))
                Log(
                    $"[Entry] The session host has a different mod version. Some features may not work properly.\n\tHost version: {hostMod.Version}\n\tLocal version: {ModManifest.Version}",
                    LogLevel.Warn);
        }

#if DEBUG
        // start FPS counter
        FpsCounter = new(GameRunner.instance);
        helper.Reflection.GetMethod(FpsCounter, "LoadContent").Invoke();
#endif
    }
}