﻿namespace DaLion.Ligo.Modules.Arsenal.Patchers.Infinity;

#region using directives

using DaLion.Ligo.Modules.Arsenal.Enchantments;
using DaLion.Shared.Harmony;
using HarmonyLib;
using StardewValley.Tools;

#endregion using directives

[UsedImplicitly]
internal sealed class ToolCanAddEnchantmentPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="ToolCanAddEnchantmentPatcher"/> class.</summary>
    internal ToolCanAddEnchantmentPatcher()
    {
        this.Target = this.RequireMethod<Tool>(nameof(Tool.CanAddEnchantment));
    }

    #region harmony patches

    /// <summary>Allow forge Galaxy with Infinity.</summary>
    [HarmonyPrefix]
    private static bool ToolCanAddEnchantmentPrefix(
        Tool __instance, ref bool __result, BaseEnchantment enchantment)
    {
        if (__instance is not Slingshot slingshot)
        {
            return true; // run original logic
        }

        if (enchantment is InfinityEnchantment && slingshot.InitialParentTileIndex == Constants.GalaxySlingshotIndex)
        {
            __result = slingshot.hasEnchantmentOfType<GalaxySoulEnchantment>() && slingshot.GetEnchantmentLevel<GalaxySoulEnchantment>() >= 3;
            return false; // don't run original logic
        }

        if (enchantment.IsForge())
        {
            __result = ArsenalModule.Config.Slingshots.AllowForges;
            return false; // don't run original logic
        }

        return true; // run original logic
    }

    #endregion harmony patches
}