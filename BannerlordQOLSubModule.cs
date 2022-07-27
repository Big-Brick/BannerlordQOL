using System;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;


namespace BannerlordQOL
{
  [HarmonyPatch(typeof(MissionMainAgentController), "ControlTick")]
  static class AttackingPatch
  {
    public static Agent.MovementControlFlag CurrentAttackType = 0U;
    public static Agent.MovementControlFlag RequestedAttackType = Agent.MovementControlFlag.AttackUp;

    public static void Postfix(MissionMainAgentController __instance)
    {
      
      try
      {
        Agent mainAgent = __instance.Mission.MainAgent;
        if (Input.IsKeyPressed(InputKey.Q))
        {
          InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.UP"));
          AttackingPatch.RequestedAttackType = Agent.MovementControlFlag.AttackUp;
        }
        else if (Input.IsKeyPressed(InputKey.Z))
          AttackingPatch.RequestedAttackType = Agent.MovementControlFlag.AttackLeft;
        else if (Input.IsKeyPressed(InputKey.X))
        {
          InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.DOWN"));
          AttackingPatch.RequestedAttackType = Agent.MovementControlFlag.AttackDown;
        }
        else if (Input.IsKeyPressed(InputKey.C))
          AttackingPatch.RequestedAttackType = Agent.MovementControlFlag.AttackRight;
        if (Input.IsKeyDown(InputKey.LeftMouseButton))
        {
          if (AttackingPatch.CurrentAttackType != 0U && AttackingPatch.CurrentAttackType != AttackingPatch.RequestedAttackType)
          {
            mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendUp;
            AttackingPatch.CurrentAttackType = 0U;
          }
          else
          {
            InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.CHANGE"));
            mainAgent.MovementFlags &= ~Agent.MovementControlFlag.AttackMask;
            mainAgent.MovementFlags |= AttackingPatch.RequestedAttackType;
            AttackingPatch.CurrentAttackType = RequestedAttackType;
          }
        }
        else if (Input.IsKeyReleased(InputKey.LeftMouseButton))
          AttackingPatch.CurrentAttackType = 0U;
      }
      catch(Exception ex)
      {
        MessageBox.Show("An exception occured while trying to process attacking\n\n" + ex.Message);
      }
    }
  }
  public class BannerlordQOLSubModule : MBSubModuleBase
  {    
    protected override void OnSubModuleLoad()
    {
      base.OnSubModuleLoad();
      try
      {
        Harmony harmonyPatch = new Harmony("bannerlord.bannerlordqol");
        harmonyPatch.PatchAll();
      }
      catch(Exception ex)
      {
        MessageBox.Show("Failed hooking BannerlordQOL attacking code\n\n" + ex.Message);
      }
    }
    
    public static bool OnFinalize(Object __instance)
    {
      return true;
    }
  }
}
