using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.GUI;

namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStart))]
public class PlayerNetworkStartHook
{
  private static void Prefix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    //if (__instance.NetworkId.Value == 1)
    //  return;

  }

  private static void Postfix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    if (__instance.NetworkId.Value == 1)
      return;

    if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
    {
      MetricsPlayer.PlayerGameObject = __instance;
      MetricsExperience.ResetExperience(__instance.Experience.Total);//TODO does this reset when switching zones?
      MetricsPlayer.IsPlayerLoadedIntoScene = true;
      ExperienceGUI.InitializeRenderObjects();


      MetricsLogging.LogMessageToConsole($"Loading in as [{MetricsPlayer.PlayerName}({MetricsPlayer.PlayerNetworkId})]. Health: {MetricsPlayer.CurrentHealth}/{MetricsPlayer.MaxHealth}");      
    }
  }
}

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStop))]
public class PlayerNetworkStopHook
{
  private static void Postfix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    if (__instance == null || __instance.NetworkId.Value == 1)
      return;

    MetricsLogging.LogMessageToConsole($"Logging Character {MetricsPlayer.PlayerName}");

    //Do clean up
    //MetricsPlayer.PlayerGameObject = null;
    MetricsPlayer.IsPlayerLoadedIntoScene = false;
      //MetricsExperience.ResetExperience();
  }

}