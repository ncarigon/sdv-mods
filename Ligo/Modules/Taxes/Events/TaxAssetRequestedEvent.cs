﻿namespace DaLion.Ligo.Modules.Taxes.Events;

#region using directives

using DaLion.Ligo.Modules.Taxes.VirtualProperties;
using DaLion.Shared.Content;
using DaLion.Shared.Events;
using DaLion.Shared.Extensions.Stardew;
using StardewModdingAPI.Events;
using static System.FormattableString;

#endregion using directives

[UsedImplicitly]
[AlwaysEnabledEvent]
internal sealed class TaxAssetRequestedEvent : AssetRequestedEvent
{
    /// <summary>Initializes a new instance of the <see cref="TaxAssetRequestedEvent"/> class.</summary>
    /// <param name="manager">The <see cref="EventManager"/> instance that manages this event.</param>
    internal TaxAssetRequestedEvent(EventManager manager)
        : base(manager)
    {
        this.Edit("Data/mail", new AssetEditor(EditMailData, AssetEditPriority.Default));
    }

    private static void EditMailData(IAssetData asset)
    {
        // patch mail from the Ferngill Revenue Service
        var data = asset.AsDictionary<string, string>().Data;

        var due = Game1.player.Get_LatestDue().ToString();
        var deductions = Game1.player.Read<float>(DataFields.PercentDeductions);
        var outstanding = Game1.player.Read(DataFields.DebtOutstanding);

        string honorific = ModEntry.i18n.Get("honorific" + (Game1.player.IsMale ? ".male" : ".female"));
        var farm = Game1.getFarm().Name;
        var interest = CurrentCulture($"{ModEntry.Config.Taxes.AnnualInterest:0%}");

        data[$"{ModEntry.Manifest.UniqueID}/TaxIntro"] =
            ModEntry.i18n.Get("tax.intro", new { honorific, farm = Game1.getFarm().Name, interest });
        data[$"{ModEntry.Manifest.UniqueID}/TaxNotice"] = ModEntry.i18n.Get("tax.notice", new { honorific, due });
        data[$"{ModEntry.Manifest.UniqueID}/TaxOutstanding"] =
            ModEntry.i18n.Get("tax.outstanding", new
            {
                honorific,
                due,
                outstanding,
                farm,
                interest,
            });
        data[$"{ModEntry.Manifest.UniqueID}/TaxDeduction"] = deductions switch
        {
            >= 1f => ModEntry.i18n.Get("tax.deduction.max", new { honorific }),
            >= 0f => ModEntry.i18n.Get(
                "tax.deduction",
                new { honorific, deductible = CurrentCulture($"{deductions:0%}") }),
            _ => string.Empty,
        };
    }
}
