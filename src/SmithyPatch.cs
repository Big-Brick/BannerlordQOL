using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
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
			if (RefinementPatch.OngoingRecursion || !Input.IsKeyDown(InputKey.LeftControl))
				return;
			RefinementPatch.OngoingRecursion = true;
			TaleWorlds.CampaignSystem.CampaignBehaviors.ICraftingCampaignBehavior CraftBehavior = Campaign.Current.GetCampaignBehavior<TaleWorlds.CampaignSystem.CampaignBehaviors.ICraftingCampaignBehavior>();
			int i = 0;
			while (i++ < 100 && __instance.CurrentSelectedAction != null && CraftBehavior.GetHeroCraftingStamina(currentCraftingHero) > 10)
				__instance.ExecuteSelectedRefinement(currentCraftingHero);
			RefinementPatch.OngoingRecursion = false;
		}
	}

	[HarmonyPatch(typeof(SmeltingVM), "SmeltSelectedItems")]
	public class SmeltingPatch
	{
		private static bool OngoingRecursion = false;
		private static int SmeltingAmount = 0;

		public static void Postfix(SmeltingVM __instance, Hero currentCraftingHero)
		{
			if (SmeltingPatch.OngoingRecursion || !Input.IsKeyDown(InputKey.LeftControl))
				return;
			SmeltingPatch.OngoingRecursion = true;
			SmeltingPatch.SmeltingAmount = 100;
			TaleWorlds.CampaignSystem.CampaignBehaviors.ICraftingCampaignBehavior CraftBehavior = Campaign.Current.GetCampaignBehavior<TaleWorlds.CampaignSystem.CampaignBehaviors.ICraftingCampaignBehavior>();
			InformationManager.DisplayMessage(new InformationMessage("Smelting postfix"));
			//ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			InformationManager.DisplayMessage(new InformationMessage("SmeltableItemList.Count " + __instance.SmeltableItemList.Count));
			int i = 0;
			while (i < __instance.SmeltableItemList.Count && SmeltingPatch.SmeltingAmount > 0 && CraftBehavior.GetHeroCraftingStamina(currentCraftingHero) > 10)
			{
				if (__instance.SmeltableItemList[i].IsLocked)
				{
					InformationManager.DisplayMessage(new InformationMessage("Skipping smelting of " + __instance.SmeltableItemList[i].Name + " because it is locked."));
					i++;
					continue;
				}
				int itemNum = __instance.SmeltableItemList[i].NumOfItems;
				InformationManager.DisplayMessage(new InformationMessage("Smelting " + itemNum + " of " + __instance.SmeltableItemList[i].Name));
				__instance.SmeltableItemList[i].ExecuteSelection();
				int j = 0;
				while (j < itemNum && SmeltingPatch.SmeltingAmount > 0 && CraftBehavior.GetHeroCraftingStamina(currentCraftingHero) > 10)
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