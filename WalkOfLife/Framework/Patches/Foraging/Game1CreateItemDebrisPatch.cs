﻿using Harmony;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;

namespace TheLion.AwesomeProfessions
{
	internal class Game1CreateItemDebrisPatch : BasePatch
	{
		/// <summary>Construct an instance.</summary>
		/// <param name="monitor">Interface for writing to the SMAPI console.</param>
		internal Game1CreateItemDebrisPatch(IMonitor monitor)
		: base(monitor) { }

		/// <summary>Apply internally-defined Harmony patches.</summary>
		/// <param name="harmony">The Harmony instance for this mod.</param>
		protected internal override void Apply(HarmonyInstance harmony)
		{
			harmony.Patch(
				AccessTools.Method(typeof(Game1), nameof(Game1.createItemDebris)),
				postfix: new HarmonyMethod(GetType(), nameof(Game1CreateItemDebrisPostfix))
			);
		}

		#region harmony patches
		/// <summary>Patch to count foraged berries as Ecologist.</summary>
		protected static void Game1CreateItemDebrisPostfix(Item item)
		{
			if (Utility.IsWildBerry(item as SObject) && Utility.LocalPlayerHasProfession("ecologist"))
				++AwesomeProfessions.Data.ItemsForaged;
		}
		#endregion harmony patches
	}
}