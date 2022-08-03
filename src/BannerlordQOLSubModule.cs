using System;
using System.Text;
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
				MessageBox.Show("Failed hooking BannerlordQOL  code\n\n" + BannerlordQOLSubModule.GetString(ex));
			}
		}

		private static string GetString(Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();

			GetStringRecursive(ex, stringBuilder);

			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Stack trace: ");
			stringBuilder.AppendLine(ex.StackTrace);

			return stringBuilder.ToString();
		}

		private static void GetStringRecursive(Exception ex, StringBuilder sb)
		{
			sb.AppendLine(ex.GetType().Name + ": ");
			sb.AppendLine(ex.Message);
			if (ex.InnerException != null)
			{
				sb.AppendLine();
				GetStringRecursive(ex.InnerException, sb);
			}
		}

		public static bool OnFinalize(Object __instance)
		{
			return true;
		}
	}
}
