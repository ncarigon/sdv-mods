﻿namespace DaLion.Overhaul.Modules.Arsenal.Patchers.Weapons;

#region using directives

using DaLion.Shared.Harmony;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Tools;
using xTile.Dimensions;

#endregion using directives

[UsedImplicitly]
internal sealed class GameLocationMonsterDropPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="GameLocationMonsterDropPatcher"/> class.</summary>
    internal GameLocationMonsterDropPatcher()
    {
        this.Target = this.RequireMethod<GameLocation>(nameof(GameLocation.monsterDrop));
    }

    #region harmony patches

    /// <summary>Drop Obsidian Edge and Lava Katana.</summary>
    [HarmonyPostfix]
    private static void GameLocationMonsterDropPostfix(GameLocation __instance, Monster monster, int x, int y, Farmer who)
    {
        if (!ArsenalModule.Config.Weapons.EnableRebalance)
        {
            return;
        }

        switch (monster)
        {
            case ShadowBrute:
            case ShadowGirl:
            case ShadowGuy:
            case ShadowShaman:
                if (Game1.mine is null)
                {
                    return;
                }

                if (Game1.mine.GetAdditionalDifficulty() > 0 && Game1.random.NextDouble() < 0.015 + (who.LuckLevel * 0.002f))
                {
                    __instance.debris.Add(new Debris(new MeleeWeapon(Constants.ObsidianEdgeIndex), new Vector2(x, y)));
                }

                break;
            case Bat bat when bat.magmaSprite.Value:
                if (Game1.random.NextDouble() < 0.01 + (who.LuckLevel * 0.003f))
                {
                    __instance.debris.Add(new Debris(new MeleeWeapon(Constants.LavaKatanaIndex), new Vector2(x, y)));
                }

                break;
        }
    }

    #endregion harmony patches
}
