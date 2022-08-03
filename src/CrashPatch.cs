using System.Windows.Forms;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.Engine;
using TaleWorlds.CampaignSystem.Party;
using SandBox.View.Map;

namespace BannerlordQOL
{
	[HarmonyPatch(typeof(PartyVisual), "RefreshPartyIcon")]
	public class RefreshPartyIconCrashPatch
	{
		public static void Prefix(PartyBase party)
		{
			if ((PartyVisual)party?.MobileParty?.CurrentSettlement?.Party?.Visuals == null)
				return;

			Dictionary<int, List<GameEntity>> entitiesWithLevels =
				Traverse.Create((PartyVisual)party?.MobileParty?.CurrentSettlement?.Party?.Visuals)
					.Field("_gateBannerEntitiesWithLevels").GetValue() as Dictionary<int, List<GameEntity>>;
			int wallLevel = 0;
			if (party?.MobileParty?.CurrentSettlement?.Town != null)
				wallLevel = party.MobileParty.CurrentSettlement.Town.GetWallLevel();
			else
				return;

			if (entitiesWithLevels == null)
				return;
			else if (wallLevel >= entitiesWithLevels.Count)
				return;
			else if (entitiesWithLevels[wallLevel].Count == 0)
			{
				if (entitiesWithLevels[1].Count != 0)
				{
					entitiesWithLevels[wallLevel] = entitiesWithLevels[1];
					Traverse.Create((PartyVisual)party?.MobileParty?.CurrentSettlement?.Party?.Visuals)
						.Field("_gateBannerEntitiesWithLevels").SetValue(entitiesWithLevels);
				}
				else if (entitiesWithLevels[2].Count != 0)
				{
					entitiesWithLevels[wallLevel] = entitiesWithLevels[2];
					Traverse.Create((PartyVisual)party?.MobileParty?.CurrentSettlement?.Party?.Visuals)
						.Field("_gateBannerEntitiesWithLevels").SetValue(entitiesWithLevels);
				}
				else if (entitiesWithLevels[3].Count != 0)
				{
					entitiesWithLevels[wallLevel] = entitiesWithLevels[3];
					Traverse.Create((PartyVisual)party?.MobileParty?.CurrentSettlement?.Party?.Visuals)
						.Field("_gateBannerEntitiesWithLevels").SetValue(entitiesWithLevels);
				}
				else if (party?.MobileParty?.CurrentSettlement?.Town != null)
				{
					string Msg = "Town.Name = " + party?.MobileParty?.CurrentSettlement?.Town.Name + " \n";
					Msg += "Town.WallLevel = " + party.MobileParty.CurrentSettlement.Town.GetWallLevel() + " \n";
					Msg += "entitiesWithLevels[1].Count = " + entitiesWithLevels[1].Count + "\n";
					Msg += "entitiesWithLevels[2].Count = " + entitiesWithLevels[2].Count + "\n";
					Msg += "entitiesWithLevels[3].Count = " + entitiesWithLevels[3].Count + "\n";
					MessageBox.Show(Msg);
				}

			}

		}
	}
}