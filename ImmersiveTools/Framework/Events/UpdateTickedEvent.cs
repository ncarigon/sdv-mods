﻿namespace DaLion.Stardew.Tools.Framework.Events;

#region using directives

using StardewModdingAPI.Events;
using StardewValley;

#endregion using directives

internal class UpdateTickedEvent : IEvent
{
    /// <inheritdoc />
    public void Hook()
    {
        ModEntry.ModHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
    }

    /// <inheritdoc />
    public void Unhook()
    {
        ModEntry.ModHelper.Events.GameLoop.UpdateTicked -= OnUpdateTicked;
    }

    /// <summary>Raised after the game state is updated.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
    {
        if (ModEntry.Shockwave.Value is null ||
            ModEntry.Config.TicksBetweenWaves > 0 && !e.IsMultipleOf(ModEntry.Config.TicksBetweenWaves)) return;
        ModEntry.Shockwave.Value.Update(Game1.currentGameTime.TotalGameTime.TotalMilliseconds);
    }
}