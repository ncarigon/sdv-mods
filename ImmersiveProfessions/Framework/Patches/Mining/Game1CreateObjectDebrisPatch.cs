﻿namespace DaLion.Stardew.Professions.Framework.Patches.Mining;

#region using directives

using System;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using StardewValley;

using Extensions;

using SObject = StardewValley.Object;

#endregion using directives

[UsedImplicitly]
internal class Game1CreateObjectDebrisPatch : BasePatch
{
    /// <summary>Construct an instance.</summary>
    internal Game1CreateObjectDebrisPatch()
    {
        Original = RequireMethod<Game1>(nameof(Game1.createObjectDebris),
            new[] {typeof(int), typeof(int), typeof(int), typeof(long), typeof(GameLocation)});
    }

    #region harmony patches

    /// <summary>Patch for Gemologist mineral quality and increment counter for mined minerals.</summary>
    [HarmonyPrefix]
    private static bool Game1CreateObjectDebrisPrefix(int objectIndex, int xTile, int yTile, long whichPlayer,
        GameLocation location)
    {
        try
        {
            var who = Game1.getFarmer(whichPlayer);
            if (!who.HasProfession(Profession.Gemologist) || !new SObject(objectIndex, 1).IsGemOrMineral())
                return true; // run original logic

            location.debris.Add(new(objectIndex, new(xTile * 64 + 32, yTile * 64 + 32),
                who.getStandingPosition())
            {
                itemQuality = who.GetGemologistMineralQuality()
            });

            who.IncrementData<uint>(DataField.GemologistMineralsCollected);
            return false; // don't run original logic
        }
        catch (Exception ex)
        {
            Log.E($"Failed in {MethodBase.GetCurrentMethod()?.Name}:\n{ex}");
            return true; // default to original logic
        }
    }

    #endregion harmony patches
}