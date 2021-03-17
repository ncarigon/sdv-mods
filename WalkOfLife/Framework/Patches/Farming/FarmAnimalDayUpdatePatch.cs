﻿using Harmony;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TheLion.Common.Harmony;

namespace TheLion.AwesomeProfessions
{
	internal class FarmAnimalDayUpdatePatch : BasePatch
	{
		private static ILHelper _helper;

		/// <summary>Construct an instance.</summary>
		/// <param name="monitor">Interface for writing to the SMAPI console.</param>
		internal FarmAnimalDayUpdatePatch(IMonitor monitor)
		: base(monitor)
		{
			_helper = new ILHelper(monitor);
		}

		/// <summary>Apply internally-defined Harmony patches.</summary>
		/// <param name="harmony">The Harmony instance for this mod.</param>
		protected internal override void Apply(HarmonyInstance harmony)
		{
			harmony.Patch(
				AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.dayUpdate)),
				transpiler: new HarmonyMethod(GetType(), nameof(FarmAnimalDayUpdateTranspiler))
			);
		}

		#region harmony patches
		/// <summary>Patch for Producer to double produce frequency at max animal happiness + remove Shepherd and Coopmaster hidden produce quality boosts.</summary>
		protected static IEnumerable<CodeInstruction> FarmAnimalDayUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			_helper.Attach(instructions).Log($"Patching method {typeof(FarmAnimal)}::{nameof(FarmAnimal.dayUpdate)}.");

			/// From: FarmeAnimal.daysToLay -= (FarmAnimal.type.Value.Equals("Sheep") && Game1.getFarmer(FarmAnimal.ownerID).professions.Contains(Farmer.shepherd)) ? 1 : 0
			/// To: FarmAnimal.daysToLay /= (FarmAnimal.happiness.Value >= 200) && Game1.getFarmer(FarmAnimal.ownerID).professions.Contains(<producer_id>) ? 2 : 1

			try
			{
				_helper
					.FindFirst(												// find index of FarmAnimal.type.Value.Equals("Sheep")
						new CodeInstruction(OpCodes.Ldstr, operand: "Sheep"),
						new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(string), nameof(string.Equals), new Type[] { typeof(string) }))
					)
					.Retreat(2)
					.SetOperand(AccessTools.Field(typeof(FarmAnimal), nameof(FarmAnimal.happiness)))													// was FarmAnimal.type
					.Advance()
					.SetOperand(AccessTools.Property(typeof(NetFieldBase<byte, NetByte>), nameof(NetFieldBase<byte, NetByte>.Value)).GetGetMethod())	// was <string, NetString>
					.Advance()
					.ReplaceWith(
						new CodeInstruction(OpCodes.Ldc_I4_S, operand: 200)	// was Ldstr "Sheep"
					)
					.Advance()
					.Remove()
					.SetOpCode(OpCodes.Blt_S)								// was Brfalse
					.AdvanceUntil(
						new CodeInstruction(OpCodes.Ldc_I4_0)
					)
					.SetOpCode(OpCodes.Ldc_I4_1)							// was Ldc_I4_0
					.Advance(2)
					.SetOpCode(OpCodes.Ldc_I4_2)							// was Ldc_I4_1
					.Advance()
					.SetOpCode(OpCodes.Div)									// was Sub
					.Advance()
					.Insert(
						new CodeInstruction(OpCodes.Conv_U1)
					);
			}
			catch (Exception ex)
			{
				_helper.Error($"Failed while patching modded Producer produce frequency.\nHelper returned {ex}").Restore();
			}

			_helper.Backup();

			/// Skipped: if ((!isCoopDweller() && Game1.getFarmer(FarmAnimal.ownerID).professions.Contains(<shepherd_id>)) || (isCoopDweller() && Game1.getFarmer(FarmAnimal.ownerID).professions.Contains(<coopmaster_id>))) chanceForQuality += 0.33

			try
			{
				_helper
					.FindNext(									// find index of first FarmAnimal.isCoopDweller check
						new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.isCoopDweller)))
					)
					.AdvanceUntil(
						new CodeInstruction(OpCodes.Brfalse)	// the all cases false branch
					)
					.GetOperand(out object resumeExecution)		// copy destination
					.Return()
					.Retreat()
					.Insert(									// insert unconditional branch to skip this whole section
						new CodeInstruction(OpCodes.Br_S, operand: (Label)resumeExecution)
					);
			}
			catch (Exception ex)
			{
				_helper.Error($"Failed while removing vanilla Coopmaster + Shepherd produce quality bonuses.\nHelper returned {ex}").Restore();
			}

			return _helper.Flush();
		}
		#endregion harmony patches
	}
}