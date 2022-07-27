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
  static class CombatPatch
  {
    public static Agent.MovementControlFlag CurrentDirection = 0U;
    public static Agent.MovementControlFlag RequestedDirection = Agent.MovementControlFlag.AttackUp | Agent.MovementControlFlag.DefendUp;

    public static void Postfix(MissionMainAgentController __instance)
    {
      
      try
      {
        Agent mainAgent = __instance.Mission.MainAgent;
        if (Input.IsKeyPressed(InputKey.Q))
        {
          InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.UP"));
          CombatPatch.RequestedDirection = Agent.MovementControlFlag.AttackUp | Agent.MovementControlFlag.DefendUp;
        }
        else if (Input.IsKeyPressed(InputKey.Z))
          CombatPatch.RequestedDirection = Agent.MovementControlFlag.AttackLeft | Agent.MovementControlFlag.DefendLeft;
        else if (Input.IsKeyPressed(InputKey.X))
        {
          InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.DOWN"));
          CombatPatch.RequestedDirection = Agent.MovementControlFlag.AttackDown | Agent.MovementControlFlag.DefendDown;
        }
        else if (Input.IsKeyPressed(InputKey.C))
          CombatPatch.RequestedDirection = Agent.MovementControlFlag.AttackRight | Agent.MovementControlFlag.DefendRight;
        if (Input.IsKeyDown(InputKey.LeftMouseButton) || Input.IsKeyDown(InputKey.RightMouseButton))
        {
          if (CombatPatch.CurrentDirection != 0U && CombatPatch.CurrentDirection != CombatPatch.RequestedDirection)
          {
            mainAgent.MovementFlags |= Input.IsKeyDown(InputKey.LeftMouseButton) ? Agent.MovementControlFlag.DefendUp : Agent.MovementControlFlag.AttackUp;
            CombatPatch.CurrentDirection = 0U;
          }
          else
          {
            InformationManager.DisplayMessage(new InformationMessage("BannerlordQOLSubModule.CHANGE"));
            mainAgent.MovementFlags &= Input.IsKeyDown(InputKey.LeftMouseButton) ? ~Agent.MovementControlFlag.AttackMask : ~Agent.MovementControlFlag.DefendMask;
            mainAgent.MovementFlags |= CombatPatch.RequestedDirection & (Input.IsKeyDown(InputKey.LeftMouseButton) ? Agent.MovementControlFlag.AttackMask : Agent.MovementControlFlag.DefendMask);
            CombatPatch.CurrentDirection = RequestedDirection;
          }
        }
        else if (Input.IsKeyReleased(InputKey.LeftMouseButton))
          CombatPatch.CurrentDirection = 0U;
      }
      catch(Exception ex)
      {
        MessageBox.Show("An exception occured while trying to process attacking\n\n" + ex.Message);
      }
    }
  }
}