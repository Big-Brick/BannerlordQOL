using HarmonyLib;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting;

namespace BannerlordQOL
{
    [HarmonyPatch(typeof(RefinementVM), "ExecuteSelectedRefinement")]
    public class RefinementPatch
    {
        private static bool repeating = false;
        
        public static void Postfix(RefinementVM __instance, Hero currentCraftingHero)
        {
            if (repeating)
                return;
            if (Input.IsKeyDown(InputKey.LeftControl))
            {
                repeating = true;
                int maxfailsafe = 100;
                while (maxfailsafe-- > 0 && __instance.CurrentSelectedAction != null)
                {
                    __instance.ExecuteSelectedRefinement(currentCraftingHero);
                }
                repeating = false;
            }
        }
    }
}