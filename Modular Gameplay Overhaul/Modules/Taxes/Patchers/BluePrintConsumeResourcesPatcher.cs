﻿namespace DaLion.Overhaul.Modules.Taxes.Patchers;

#region using directives

using DaLion.Shared.Extensions.Stardew;
using DaLion.Shared.Harmony;
using HarmonyLib;

#endregion using directives

[UsedImplicitly]
internal sealed class BluePrintConsumeResourcesPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="BluePrintConsumeResourcesPatcher"/> class.</summary>
    internal BluePrintConsumeResourcesPatcher()
    {
        this.Target = this.RequireMethod<BluePrint>(nameof(BluePrint.consumeResources));
    }

    #region harmony patches

    /// <summary>Patch to deduct building expenses.</summary>
    [HarmonyPostfix]
    private static void BluePrintConsumeResourcesPostfix(BluePrint __instance)
    {
        if (!TaxesModule.Config.DeductibleBuildingExpenses)
        {
            return;
        }

        if (TaxesModule.PlayerShouldPayTaxes)
        {
            Game1.player.Increment(DataFields.BusinessExpenses, __instance.moneyRequired);
        }
        else
        {
            ModEntry.Broadcaster.MessageHost(__instance.moneyRequired.ToString(), "Taxes." + DataFields.BusinessExpenses);
        }
    }

    #endregion harmony patches
}
