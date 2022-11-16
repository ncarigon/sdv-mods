﻿namespace DaLion.Ligo.Modules.Rings;

#region using directives

using Microsoft.Xna.Framework.Graphics;

#endregion using directives

/// <summary>Caches custom mod textures and related functions.</summary>
internal static class Textures
{
    internal static Texture2D RingsTx { get; } =
        ModEntry.ModHelper.ModContent.Load<Texture2D>("assets/sprites/rings");

    internal static Texture2D GemstonesTx { get; } =
        ModEntry.ModHelper.ModContent.Load<Texture2D>("assets/sprites/gemstones");

    internal static Texture2D ResonanceLightTx { get; } =
        ModEntry.ModHelper.ModContent.Load<Texture2D>("assets/lights/resonance");
}