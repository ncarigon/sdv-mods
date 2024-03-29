﻿namespace DaLion.Overhaul.Modules.Rings.Integrations;

#region using directives

using DaLion.Shared.Attributes;
using DaLion.Shared.Integrations;
using DaLion.Shared.Integrations.WearMoreRings;

#endregion using directives

[RequiresMod("bcmpinc.WearMoreRings", "Wear More Rings", "5.1")]
internal sealed class WearMoreRingsIntegration : ModIntegration<WearMoreRingsIntegration, IWearMoreRingsApi>
{
    private WearMoreRingsIntegration()
        : base("bcmpinc.WearMoreRings", "Wear More Rings", "5.1", ModHelper.ModRegistry)
    {
    }
}
