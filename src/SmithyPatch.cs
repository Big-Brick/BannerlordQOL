using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting;
using TaleWorlds.Library;

namespace BannerlordQOL
{
	[HarmonyPatch(typeof(RefinementVM), "ExecuteSelectedRefinement")]
	public class RefinementPatch
	{
		private static bool OngoingRecursion = false;

		public static void Postfix(RefinementVM __instance, Hero currentCraftingHero)
		{
			if (RefinementPatch.OngoingRecursion)
				return;
			if (Input.IsKeyDown(InputKey.LeftControl))
			{
				RefinementPatch.OngoingRecursion = true;
				int i = 0;
				while (i++ < 100 && __instance.CurrentSelectedAction != null)
				{
					__instance.ExecuteSelectedRefinement(currentCraftingHero);
				}
				RefinementPatch.OngoingRecursion = false;
			}
		}
	}

	[HarmonyPatch(typeof(SmeltingVM), "SmeltSelectedItems")]
	public class SmeltingPatch
	{
		private static bool OngoingRecursion = false;
		private static int SmeltingAmount = 0;

		public static void Postfix(SmeltingVM __instance, Hero currentCraftingHero)
		{
			if (Input.IsKeyDown(InputKey.LeftControl) && !SmeltingPatch.OngoingRecursion)
			{
				SmeltingPatch.OngoingRecursion = true;
				SmeltingPatch.SmeltingAmount = 100;
				InformationManager.DisplayMessage(new InformationMessage("Smelting postfix"));
				List<string> LockedItemIDs = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks().ToList<string>();
				InformationManager.DisplayMessage(new InformationMessage("Smelting postfix Got locks"));
				//ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
				int i = 0;
				while (i < MobileParty.MainParty.ItemRoster.Count && SmeltingPatch.SmeltingAmount > 0)
				{
					if (!MobileParty.MainParty.ItemRoster[i].EquipmentElement.Item.IsCraftedWeapon)
					{
						i++;
						continue;
					}
					if (LockedItemIDs.Contains(CampaignUIHelper.GetItemLockStringID(MobileParty.MainParty.ItemRoster[i].EquipmentElement)))
					{
						i++;
						continue;
					}
					int itemNum = MobileParty.MainParty.ItemRoster[i].Amount;
					InformationManager.DisplayMessage(new InformationMessage("Smelting " + itemNum + " of " + MobileParty.MainParty.ItemRoster[i].EquipmentElement.ToString()));
					int j = 0;
					while (j < itemNum && SmeltingPatch.SmeltingAmount > 0)
					{
						__instance.TrySmeltingSelectedItems(currentCraftingHero);
						j++;
						SmeltingPatch.SmeltingAmount--;
					}
					i++;
				}
				SmeltingPatch.OngoingRecursion = false;
			}
		}
	}
}