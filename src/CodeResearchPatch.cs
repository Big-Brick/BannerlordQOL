using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.Library;

namespace BannerlordQOL
{
	[HarmonyPatch(typeof(CraftingCampaignBehavior), "CreateCraftedWeaponInCraftingOrderMode")]
	public class CraftingOrderResearchPatch
	{
		public static ItemObject Postfix(
			ItemObject __result)
		{
			InformationManager.DisplayMessage(new InformationMessage("CreateCraftedWeaponInCraftingOrderMode Postfix for" + __result.Name));
			InformationManager.DisplayMessage(new InformationMessage("item.IsTradeGood " + __result.IsTradeGood + " item.IsAnimal " + __result.IsAnimal));
			InformationManager.DisplayMessage(new InformationMessage("Item.Value " + __result.Value));
			return __result;
		}
	}

	[HarmonyPatch(typeof(CraftingCampaignBehavior), "CreateCraftedWeaponInFreeBuildMode")]
	public class FreeBuildesearchPatch
	{
		public static ItemObject Postfix(
			ItemObject __result)
		{
			InformationManager.DisplayMessage(new InformationMessage("CreateCraftedWeaponInCraftingOrderMode Postfix for" + __result.Name));
			InformationManager.DisplayMessage(new InformationMessage("item.IsTradeGood " + __result.IsTradeGood + " item.IsAnimal " + __result.IsAnimal));
			InformationManager.DisplayMessage(new InformationMessage("Item.Value " + __result.Value));
			return __result;
		}
	}
}