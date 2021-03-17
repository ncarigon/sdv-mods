﻿using Harmony;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TheLion.Common.Harmony;

namespace TheLion.AwesomeProfessions
{
	internal class FarmAnimalPetPatch : BasePatch
	{
		private static ILHelper _helper;

		/// <summary>Construct an instance.</summary>
		/// <param name="monitor">Interface for writing to the SMAPI console.</param>
		internal FarmAnimalPetPatch(IMonitor monitor)
		: base(monitor)
		{
			_helper = new ILHelper(monitor);
		}

		/// <summary>Apply internally-defined Harmony patches.</summary>
		/// <param name="harmony">The Harmony instance for this mod.</param>
		protected internal override void Apply(HarmonyInstance harmony)
		{
			harmony.Patch(
				AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.pet)),
				transpiler: new HarmonyMethod(GetType(), nameof(FarmAnimalPetTranspiler))
			);
		}

		#region harmony patches
		/// <summary>Patch for Rancher to combine Shepherd and Coopmaster friendship bonus.</summary>
		protected static IEnumerable<CodeInstruction> FarmAnimalPetTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			_helper.Attach(instructions).Log($"Patching method {typeof(FarmAnimal)}::{nameof(FarmAnimal.pet)}.");

			/// From: if ((who.professions.Contains(<shepherd_id>) && !isCoopDweller()) || (who.professions.Contains(<coopmaster_id>) && isCoopDweller()))
			/// To: if (who.professions.Contains(<rancher_id>)

			try
			{
				_helper
					.FindProfessionCheck(Farmer.shepherd)									// find index of shepherd check
					.Advance()
					.SetOpCode(OpCodes.Ldc_I4_0)											// replace with rancher check
					.Advance(2)																// the false case branch instruction
					.AdvanceUntil(
						new CodeInstruction(OpCodes.Brfalse)								// the true case branch instruction
					)
					.GetOperand(out object hasRancher)										// copy destination
					.Return()
					.ReplaceWith(
						new CodeInstruction(OpCodes.Brtrue_S, operand: (Label)hasRancher)	// replace false case branch with true case branch
					)
					.Advance()
					.FindProfessionCheck(Farmer.butcher, fromCurrentIndex: true)			// find coopmaster check
					.Advance(3)																// the branch to resume execution
					.GetOperand(out object resumeExecution)									// copy destination
					.Return(2)
					.Insert(
						new CodeInstruction(OpCodes.Br_S, operand: (Label)resumeExecution)	// insert new false case branch
					);
			}
			catch (Exception ex)
			{
				_helper.Error($"Failed while moving combined vanilla Coopmaster + Shepherd friendship bonuses to Rancher.\nHelper returned {ex}").Restore();
			}

			return _helper.Flush();
		}
		#endregion harmony patches
	}
}