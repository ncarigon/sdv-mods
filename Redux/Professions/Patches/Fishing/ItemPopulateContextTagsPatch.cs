﻿namespace DaLion.Redux.Professions.Patches.Fishing;

#region using directives

using System.Collections.Generic;
using HarmonyLib;
using HarmonyPatch = DaLion.Shared.Harmony.HarmonyPatch;

#endregion using directives

[UsedImplicitly]
internal sealed class ItemPopulateContextTagsPatch : HarmonyPatch
{
    /// <summary>Initializes a new instance of the <see cref="ItemPopulateContextTagsPatch"/> class.</summary>
    internal ItemPopulateContextTagsPatch()
    {
        this.Target = this.RequireMethod<Item>("_PopulateContextTags");
    }

    #region harmony patches

    /// <summary>Patch to add pair context tag to extended family fish.</summary>
    [HarmonyPostfix]
    private static void ItemPopulateContextTagsPostfix(Item __instance, HashSet<string> tags)
    {
        if (!Collections.ExtendedFamilyPairs.TryGetValue(__instance.ParentSheetIndex, out var pairId))
        {
            return;
        }

        var pair = new SObject(pairId, 1);
        tags.Add("item_" + __instance.SanitizeContextTag(pair.Name));
    }

    #endregion harmony patches
}