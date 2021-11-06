﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using TheLion.Stardew.Common.Harmony;
using TheLion.Stardew.Professions.Framework.Extensions;
using SObject = StardewValley.Object;

namespace TheLion.Stardew.Professions.Framework.Patches
{
	internal class ProducerRuleControllerProduceOutputPatch : BasePatch
	{
		/// <summary>Construct an instance.</summary>
		internal ProducerRuleControllerProduceOutputPatch()
		{
			Original = AccessTools.Method("ProducerFrameworkMod.Controllers.ProducerRuleController:ProduceOutput");
		}

		#region harmony patches

		/// <summary>Patch to apply modded Gemologist quality rules to PFM Crystalariums.</summary>
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> ProducerRuleControllerProduceOutputTranspiler(
			IEnumerable<CodeInstruction> instructions, MethodBase original)
		{
			var helper = new ILHelper(original, instructions);

			/// Injected: output = IncreaseQualityIfNecessary(output, producer, who)
			/// After: output = OutputConfigController.CreateOutput( ... )
			/// Before: producer.heldObject.set_Value(output)

			try
			{
				helper
					.FindFirst( // find index of setting producer held object value
						new CodeInstruction(OpCodes.Ldloc_S, $"{typeof(SObject)} (5)"),
						new CodeInstruction(OpCodes.Callvirt,
							typeof(NetFieldBase<SObject, NetRef<SObject>>).MethodNamed("set_Value",
								new[] {typeof(SObject)}))
					)
					.Insert(
						// load producer instance
						new CodeInstruction(OpCodes.Ldarg_1), // arg 1 = SObject producer
						// load Farmer who
						new CodeInstruction(OpCodes.Ldarg_3), // arg 3 = Farmer who
						// call custom logic
						new CodeInstruction(OpCodes.Call,
							typeof(ProducerRuleControllerProduceOutputPatch).MethodNamed(
								nameof(ProduceOutputSubroutine)))
					);
			}
			catch (Exception ex)
			{
				ModEntry.Log(
					$"Failed while patching PFM for Gemologist Crystalariume output quality.\nHelper returned {ex}",
					LogLevel.Error);
				return null;
			}

			return helper.Flush();
		}

		#endregion harmony patches

		#region private methods

		private static SObject ProduceOutputSubroutine(SObject output, SObject producer, Farmer who)
		{
			if (!producer.heldObject.Value.IsForagedMineral() && !producer.heldObject.Value.IsGemOrMineral() ||
			    !who.HasProfession("Gemologist")) return output;

			output.Quality = Utility.Professions.GetGemologistMineralQuality();
			if (who.IsLocalPlayer) ModEntry.Data.IncrementField<uint>("MineralsCollected");

			return output;
		}

		#endregion private methods
	}
}