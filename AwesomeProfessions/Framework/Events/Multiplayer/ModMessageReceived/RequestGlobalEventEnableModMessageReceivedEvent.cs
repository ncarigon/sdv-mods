﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using TheLion.Stardew.Professions.Framework.Events.GameLoop.DayEnding;

namespace TheLion.Stardew.Professions.Framework.Events.Multiplayer.ModMessageReceived;

internal class RequestGlobalEventEnableModMessageReceivedEvent : ModMessageReceivedEvent
{
    /// <inheritdoc />
    protected override void OnModMessageReceivedImpl(object sender, ModMessageReceivedEventArgs e)
    {
        if (e.FromModID != ModEntry.Manifest.UniqueID || !e.Type.StartsWith("RequestEventEnable")) return;

        var which = e.ReadAs<string>();
        var who = Game1.getFarmer(e.FromPlayerID);
        if (who is null)
        {
            ModEntry.Log($"Unknown player {e.FromPlayerID} requested {which} event subscription.", LogLevel.Warn);
            return;
        }

        switch (which)
        {
            case "Conservationist":
                ModEntry.Log($"Player {e.FromPlayerID} requested {which} event subscription.", ModEntry.DefaultLogLevel);
                ModEntry.EventManager.Enable(typeof(GlobalConservationistDayEndingEvent));
                break;
        }
    }
}