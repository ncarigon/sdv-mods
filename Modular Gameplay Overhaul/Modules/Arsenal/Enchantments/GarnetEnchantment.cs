﻿namespace DaLion.Overhaul.Modules.Arsenal.Enchantments;

#region using directives

using System.Xml.Serialization;
using DaLion.Overhaul.Modules.Arsenal.Extensions;
using StardewValley.Tools;

#endregion using directives

/// <summary>The Garnet gemstone forge.</summary>
[XmlType("Mods_DaLion_GarnetEnchantment")]
public class GarnetEnchantment : BaseWeaponEnchantment
{
    /// <inheritdoc />
    public override bool ShouldBeDisplayed()
    {
        return false;
    }

    /// <inheritdoc />
    public override bool IsForge()
    {
        return true;
    }

    /// <inheritdoc />
    protected override void _ApplyTo(Item item)
    {
        base._ApplyTo(item);
    }

    /// <inheritdoc />
    protected override void _UnapplyTo(Item item)
    {
        base._UnapplyTo(item);
        if (item is Tool tool and (MeleeWeapon or Slingshot))
        {
            tool.Invalidate();
        }
    }
}
