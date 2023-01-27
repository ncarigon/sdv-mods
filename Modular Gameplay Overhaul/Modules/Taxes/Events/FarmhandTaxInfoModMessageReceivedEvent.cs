namespace DaLion.Overhaul.Modules.Taxes.Events;

#region using directives

using DaLion.Shared.Events;
using DaLion.Shared.Extensions.Stardew;
using StardewModdingAPI.Events;
using StardewValley;

#endregion using directives

[UsedImplicitly]
internal sealed class FarmhandTaxInfoModMessageReceivedEvent : ModMessageReceivedEvent
{
    /// <summary>Initializes a new instance of the <see cref="FarmhandTaxInfoModMessageReceivedEvent"/> class.</summary>
    /// <param name="manager">The <see cref="EventManager"/> instance that manages this event.</param>
    internal FarmhandTaxInfoModMessageReceivedEvent(EventManager manager)
        : base(manager)
    {
    }

    /// <inheritdoc />
    public override bool IsEnabled => Context.IsMultiplayer && Context.IsMainPlayer;

    /// <inheritdoc />
    protected override void OnModMessageReceivedImpl(object? sender, ModMessageReceivedEventArgs e)
    {
        if (string.Compare(e.Type?[..6], "Taxes.") != 0)
        {
            return;
        }

        var field = e?.Type?[6..];
        if (string.IsNullOrWhiteSpace(field))
        {
            return;
        }

        var money = e?.ReadAs<int>() ?? 0;
        if (money == 0)
        {
            return;
        }

        Log.I($"A farmhand has incremented {Game1.player.Name}'s {field} by {money}.");
        Game1.player.Increment(field, money);
    }
}
