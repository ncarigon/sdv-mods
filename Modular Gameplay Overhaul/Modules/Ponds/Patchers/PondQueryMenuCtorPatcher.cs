﻿namespace DaLion.Overhaul.Modules.Ponds.Patchers;

#region using directives

using DaLion.Shared.Extensions;
using DaLion.Shared.Extensions.Stardew;
using DaLion.Shared.Harmony;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.Menus;

#endregion using directives

[UsedImplicitly]
internal sealed class PondQueryMenuCtorPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="PondQueryMenuCtorPatcher"/> class.</summary>
    internal PondQueryMenuCtorPatcher()
    {
        this.Target = this.RequireConstructor<PondQueryMenu>(typeof(FishPond));
    }

    #region harmony patches

    /// <summary>Handle invalid data on menu open.</summary>
    [HarmonyPrefix]
    private static void PondQueryMenuCtorPrefix(FishPond fish_pond)
    {
        try
        {
            fish_pond.Read(DataFields.FishQualities).ParseTuple<int, int, int, int>();
        }
        catch (InvalidOperationException ex)
        {
            Log.W($"FishQualities data is invalid. {ex}\nThe data will be reset");
            fish_pond.Write(DataFields.FishQualities, $"{fish_pond.FishCount},0,0,0");
            fish_pond.Write(DataFields.FamilyQualities, null);
            fish_pond.Write(DataFields.FamilyLivingHere, null);
        }

        try
        {
            fish_pond.Read(DataFields.FamilyQualities).ParseTuple<int, int, int, int>();
        }
        catch (InvalidOperationException ex)
        {
            Log.W($"FamilyQuality data is invalid. {ex}\nThe data will be reset");
            fish_pond.Write(DataFields.FishQualities, $"{fish_pond.FishCount},0,0,0");
            fish_pond.Write(DataFields.FamilyQualities, null);
            fish_pond.Write(DataFields.FamilyLivingHere, null);
        }
    }

    #endregion harmony patches
}
