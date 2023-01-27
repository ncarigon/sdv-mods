namespace DaLion.Overhaul.Modules.Taxes.Patchers;

#region using directives

using DaLion.Shared.Extensions.Stardew;
using DaLion.Shared.Harmony;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

#endregion using directives

[UsedImplicitly]
internal sealed class PurchaseAnimalsSetUpForReturnAfterPurchasingAnimalPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="PurchaseAnimalsSetUpForReturnAfterPurchasingAnimalPatcher"/> class.</summary>
    internal PurchaseAnimalsSetUpForReturnAfterPurchasingAnimalPatcher()
    {
        this.Target = this.RequireMethod<PurchaseAnimalsMenu>(nameof(PurchaseAnimalsMenu.setUpForReturnAfterPurchasingAnimal));
    }

    #region harmony patches

    /// <summary>Patch to deduct animal expenses.</summary>
    [HarmonyPostfix]
    private static void PurchaseAnimalsMenuReceiveLeftClickPostfix(PurchaseAnimalsMenu __instance, int ___priceOfAnimal)
    {
        if (!TaxesModule.Config.DeductibleAnimalExpenses)
        {
            return;
        }

        if (TaxesModule.PlayerShouldPayTaxes)
        {
            Game1.player.Increment(DataFields.BusinessExpenses, ___priceOfAnimal);
        }
        else
        {
            ModEntry.Broadcaster.MessageHost(___priceOfAnimal.ToString(), "Taxes." + DataFields.BusinessExpenses);
        }
    }

    #endregion harmony patches
}
