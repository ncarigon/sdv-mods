﻿namespace DaLion.Overhaul.Modules.Arsenal.Integrations;

#region using directives

using DaLion.Shared.Attributes;
using DaLion.Shared.Extensions.SMAPI;
using DaLion.Shared.Integrations;

#endregion using directives

[RequiresMod("Taiyo.VanillaTweaks", "Vanilla Tweaks")]
internal sealed class VanillaTweaksIntegration : ModIntegration<VanillaTweaksIntegration>
{
    private VanillaTweaksIntegration()
        : base("Taiyo.VanillaTweaks", "Vanilla Tweaks", null, ModHelper.ModRegistry)
    {
    }

    /// <summary>Gets a value indicating whether the <c>RingsCategoryEnabled</c> config setting is enabled.</summary>
    internal bool WeaponsCategoryEnabled { get; private set; }

    /// <inheritdoc />
    protected override bool RegisterImpl()
    {
        if (!this.IsLoaded)
        {
            return false;
        }

        if (ModHelper.ReadContentPackConfig("Taiyo.VanillaTweaks") is { } jObject)
        {
            this.WeaponsCategoryEnabled = jObject.Value<bool>("WeaponsCategoryEnabled") == true;
            ModHelper.GameContent.InvalidateCache("TileSheets/weapons");
            return true;
        }

        Log.W("Failed to read Vanilla Tweaks config settings.");
        return false;
    }
}
