using System;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.MountAndBlade;


namespace BannerlordQOL
{
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
