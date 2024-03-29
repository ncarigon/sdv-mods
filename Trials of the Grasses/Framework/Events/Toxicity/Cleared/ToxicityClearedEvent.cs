﻿namespace DaLion.Stardew.Alchemy.Framework.Events.Toxicity;

#region using directives

using Common.Events;
using System;

#endregion using directives

/// <summary>A dynamic event raised when a player's Toxicity value drops back to zero.</summary>
internal class ToxicityClearedEvent : ManagedEvent
{
    protected readonly Action<object?, IToxicityClearedEventArgs> _OnChargeInitiatedImpl;

    /// <summary>Construct an instance.</summary>
    /// <param name="callback">The delegate to run when the event is raised.</param>
    internal ToxicityClearedEvent(Action<object?, IToxicityClearedEventArgs> callback)
        : base(ModEntry.EventManager)
    {
        _OnChargeInitiatedImpl = callback;
    }

    /// <summary>Raised when a player's Toxicity value drops back to zero.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    internal void OnCleared(object? sender, IToxicityClearedEventArgs e)
    {
        if (IsEnabled) _OnChargeInitiatedImpl(sender, e);
    }
}