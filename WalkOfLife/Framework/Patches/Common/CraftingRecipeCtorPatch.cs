﻿using System;
using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using TheLion.Stardew.Common.Harmony;
using TheLion.Stardew.Professions.Framework.Extensions;

namespace TheLion.Stardew.Professions.Framework.Patches
{
	internal class CraftingRecipeCtorPatch : BasePatch
	{
		/// <summary>Construct an instance.</summary>
		internal CraftingRecipeCtorPatch()
		{
			Original = typeof(CraftingRecipe).Constructor(new[] {typeof(string), typeof(bool)});
			Postfix = new(GetType(), nameof(CraftingRecipeCtorPostfix));
		}

		#region harmony patches

		/// <summary>Patch for cheaper crafting recipes for Blaster and Tapper.</summary>
		[HarmonyPostfix]
		private static void CraftingRecipeCtorPostfix(ref CraftingRecipe __instance)
		{
			try
			{
				if (__instance.name == "Tapper" && Game1.player.HasProfession("Tapper"))
					__instance.recipeList = new()
					{
						{388, 25}, // wood
						{334, 1} // copper bar
					};
				else if (__instance.name.Contains("Bomb") && Game1.player.HasProfession("Blaster"))
					__instance.recipeList = __instance.name switch
					{
						"Cherry Bomb" => new()
						{
							{378, 2}, // copper ore
							{382, 1} // coal
						},
						"Bomb" => new()
						{
							{380, 2}, // iron ore
							{382, 1} // coal
						},
						"Mega Bomb" => new()
						{
							{384, 2}, // gold ore
							{382, 1} // coal
						},
						_ => __instance.recipeList
					};
			}
			catch (Exception ex)
			{
				Log($"Failed in {MethodBase.GetCurrentMethod()?.Name}:\n{ex}", LogLevel.Error);
			}
		}

		#endregion harmony patches
	}
}