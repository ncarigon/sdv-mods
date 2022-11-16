﻿namespace DaLion.Ligo.Modules.Professions.Patches.Integrations;

#region using directives

using DaLion.Shared.Attributes;
using HarmonyLib;
using Shared.Harmony;

#endregion using directives

[UsedImplicitly]
[Integration("spacechase0.SpaceCore")]
internal sealed class SkillsAddExperiencePatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="SkillsAddExperiencePatcher"/> class.</summary>
    internal SkillsAddExperiencePatcher()
    {
        this.Target = this.RequireMethod<SpaceCore.Skills>(nameof(SpaceCore.Skills.AddExperience));
    }

    #region harmony patches

    /// <summary>Patch to apply prestige exp multiplier to custom skills.</summary>
    [HarmonyPrefix]
    private static void SkillsAddExperiencePrefix(string skillName, ref int amt)
    {
        if (!ModEntry.Config.Professions.EnablePrestige || !SCSkill.Loaded.TryGetValue(skillName, out var skill) ||
            amt <= 0)
        {
            return;
        }

        amt = (int)(amt * skill.BaseExperienceMultiplier * skill.PrestigeExperienceMultiplier);
    }

    #endregion harmony patches
}