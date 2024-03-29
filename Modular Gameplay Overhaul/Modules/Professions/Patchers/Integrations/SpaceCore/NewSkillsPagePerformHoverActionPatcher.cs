﻿namespace DaLion.Overhaul.Modules.Professions.Patchers.Integrations;

#region using directives

using System.Linq;
using DaLion.Overhaul.Modules.Professions.Extensions;
using DaLion.Overhaul.Modules.Professions.Integrations;
using DaLion.Shared.Attributes;
using DaLion.Shared.Extensions;
using DaLion.Shared.Harmony;
using HarmonyLib;
using Microsoft.Xna.Framework;
using SpaceCore.Interface;
using StardewValley.Menus;

#endregion using directives

[UsedImplicitly]
[RequiresMod("spacechase0.SpaceCore")]
internal sealed class NewSkillsPagePerformHoverActionPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="NewSkillsPagePerformHoverActionPatcher"/> class.</summary>
    internal NewSkillsPagePerformHoverActionPatcher()
    {
        this.Target = this.RequireMethod<NewSkillsPage>(nameof(NewSkillsPage.performHoverAction));
    }

    #region harmony patches

    /// <summary>Patch to add prestige ribbon hover text + truncate profession descriptions in hover menu.</summary>
    [HarmonyPostfix]
    private static void NewSkillsPagePerformHoverActionPostfix(
        NewSkillsPage __instance, int x, int y, ref string ___hoverText)
    {
        ___hoverText = ___hoverText.Truncate(90);

        if (!ProfessionsModule.Config.EnablePrestige)
        {
            return;
        }

        var bounds = ProfessionsModule.Config.PrestigeProgressionStyle switch
        {
            Config.ProgressionStyle.StackedStars => new Rectangle(
                __instance.xPositionOnScreen + __instance.width + Textures.ProgressionHorizontalOffset - 22,
                __instance.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Textures.ProgressionVerticalOffset + 8,
                0,
                (int)(Textures.SingleStarWidth * Textures.StarsScale)),
            Config.ProgressionStyle.Gen3Ribbons or Config.ProgressionStyle.Gen4Ribbons => new Rectangle(
                __instance.xPositionOnScreen + __instance.width + Textures.ProgressionHorizontalOffset,
                __instance.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Textures.ProgressionVerticalOffset,
                (int)(Textures.RibbonWidth * Textures.RibbonScale),
                (int)(Textures.RibbonWidth * Textures.RibbonScale)),
            _ => Rectangle.Empty,
        };

        for (var i = 0; i < 5; i++)
        {
            bounds.Y += 56;

            // need to do this bullshit switch because mining and fishing are inverted in the skills page
            var skill = i switch
            {
                1 => Skill.Mining,
                3 => Skill.Fishing,
                _ => Skill.FromValue(i),
            };
            var professionsForThisSkill = Game1.player.GetProfessionsForSkill(skill, true);
            var count = professionsForThisSkill.Length;
            if (count == 0)
            {
                continue;
            }

            bounds.Width = ProfessionsModule.Config.PrestigeProgressionStyle is Config.ProgressionStyle.Gen3Ribbons
                or Config.ProgressionStyle.Gen4Ribbons
                ? (int)(Textures.RibbonWidth * Textures.RibbonScale)
                : (int)(((Textures.SingleStarWidth / 2 * count) + 4) * Textures.StarsScale);
            if (!bounds.Contains(x, y))
            {
                continue;
            }

            ___hoverText = I18n.Get("prestige.skillpage.tooltip", new { count });
            for (var j = 0; j < professionsForThisSkill.Length; j++)
            {
                ___hoverText += $"\n• {professionsForThisSkill[j].Title}";
            }
        }

        if (SCSkill.Loaded.Count == 0)
        {
            return;
        }

        var customSkills = SpaceCoreIntegration.Instance!.ModApi!
            .GetCustomSkills()
            .Select(name => SCSkill.Loaded[name]);
        if (LuckSkill.Instance is not null)
        {
            // luck skill must be enumerated first
            customSkills = customSkills.Prepend(LuckSkill.Instance);
        }

        foreach (var skill in customSkills)
        {
            bounds.Y += 56;

            var professionsForThisSkill =
                Game1.player.GetProfessionsForSkill(skill, true);
            var count = professionsForThisSkill.Length;
            if (count == 0)
            {
                continue;
            }

            bounds.Width = ProfessionsModule.Config.PrestigeProgressionStyle is Config.ProgressionStyle.Gen3Ribbons
                or Config.ProgressionStyle.Gen4Ribbons
                ? (int)(Textures.RibbonWidth * Textures.RibbonScale)
                : (int)(((Textures.SingleStarWidth / 2 * count) + 4) * Textures.StarsScale);
            if (!bounds.Contains(x, y))
            {
                continue;
            }

            ___hoverText = I18n.Get("prestige.skillpage.tooltip", new { count });
            for (var j = 0; j < professionsForThisSkill.Length; j++)
            {
                ___hoverText += $"\n• {professionsForThisSkill[j].Title}";
            }
        }
    }

    #endregion harmony patches
}
