﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.IO;
using System.Reflection;
using TheLion.Stardew.Common.Extensions;
using TheLion.Stardew.Common.Harmony;
using SObject = StardewValley.Object;

namespace TheLion.Stardew.Professions.Framework.Patches
{
	internal class FarmerShowItemIntakePatch : BasePatch
	{
		/// <summary>Construct an instance.</summary>
		internal FarmerShowItemIntakePatch()
		{
			Original = typeof(Farmer).MethodNamed(nameof(Farmer.showItemIntake));
			Prefix = new HarmonyMethod(GetType(), nameof(FarmerShowItemIntakePrefix));
		}

		#region harmony patches

		/// <summary>Patch to show weapons during crab pot harvest animation.</summary>
		[HarmonyPrefix]
		private static bool FarmerShowItemIntakePrefix(Farmer who)
		{
			try
			{
				if (!who.mostRecentlyGrabbedItem.ParentSheetIndex.AnyOf(14, 51)) return true; // run original logic

				var toShow = (SObject)who.mostRecentlyGrabbedItem;
				var tempSprite = who.FacingDirection switch
				{
					2 => who.FarmerSprite.currentAnimationIndex switch
					{
						1 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -32f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						2 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -43f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						3 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -128f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						4 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -120f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						5 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -120f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, 4f, -0.02f, 0f, 0f),
						_ => null
					},
					1 => who.FarmerSprite.currentAnimationIndex switch
					{
						1 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(28f, -64f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						2 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(24f, -72f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						3 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(4f, -128f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						4 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -124f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						5 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -124f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, 4f, -0.02f, 0f, 0f),
						_ => null
					},
					0 => who.FarmerSprite.currentAnimationIndex switch
					{
						1 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -32f), flicker: false, flipped: false,
							who.getStandingY() / 10000f - 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f),
						2 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -43f), flicker: false, flipped: false,
							who.getStandingY() / 10000f - 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f),
						3 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(0f, -128f), flicker: false, flipped: false,
							who.getStandingY() / 10000f - 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f),
						4 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -120f), flicker: false, flipped: false,
							who.getStandingY() / 10000f - 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f),
						5 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -120f), flicker: false, flipped: false,
							who.getStandingY() / 10000f - 0.001f, 0.02f, Color.White, 4f, -0.02f, 0f, 0f),
						_ => null
					},
					3 => who.FarmerSprite.currentAnimationIndex switch
					{
						1 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(-32f, -64f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						2 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(-28f, -76f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						3 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							100f, 1, 0, who.Position + new Vector2(-16f, -128f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						4 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -124f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f),
						5 => new TemporaryAnimatedSprite(Path.Combine("TileSheets", "weapons"),
							Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, toShow.ParentSheetIndex, 16, 16),
							200f, 1, 0, who.Position + new Vector2(0f, -124f), flicker: false, flipped: false,
							who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, 4f, -0.02f, 0f, 0f),
						_ => null
					},
					_ => null
				};

				if ((toShow.Equals(who.ActiveObject) || (who.ActiveObject != null && toShow != null && toShow.ParentSheetIndex == who.ActiveObject.ParentSheetIndex)) && who.FarmerSprite.currentAnimationIndex == 5)
					tempSprite = null;

				if (tempSprite != null) who.currentLocation.temporarySprites.Add(tempSprite);

				if (who.FarmerSprite.currentAnimationIndex != 5) return false; // don't run original logic

				who.Halt();
				who.FarmerSprite.CurrentAnimation = null;
				return false; // don't run original logic
			}
			catch (Exception ex)
			{
				ModEntry.Log($"Failed in {MethodBase.GetCurrentMethod().Name}:\n{ex}", LogLevel.Error);
				return true; // default to original logic
			}
		}

		#endregion harmony patches
	}
}