using HarmonyLib;
using Il2Cpp;
using Il2CppViNL;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.GUI;

namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStart))]
public class PlayerNetworkStartHook
{
  public static bool exitMessageReceieved { get; set; } = false;


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
      MetricsPlayer.ResetPets();
      MetricsExperience.ResetExperience(__instance.Experience.Total);//TODO does this reset when switching zones?
      MetricsPlayer.IsPlayerLoadedIntoScene = true;
      //ExperienceGUI.InitializeRenderObjects();
      GuiLeftBar.InitializeRenderObjects();
      MetricsCombat.ResetDpsMeter();

      MetricsLogging.LogMessageToConsole($"Loading in as [{MetricsPlayer.PlayerName}({MetricsPlayer.PlayerNetworkId})]. Health: {MetricsPlayer.CurrentHealth}/{MetricsPlayer.MaxHealth}");
      exitMessageReceieved = false;
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

    MetricsLogging.LogMessageToConsole($"Character {MetricsPlayer.PlayerName} has logged.");

    MetricsPlayer.IsPlayerLoadedIntoScene = false;


    //Sometimes we randomly receive a NetworkStop even though we are still in scene. Setting this to true, and then checking in SetOverride if we reveive messages. If we do it was a false Stop and we should set IsPlayerLoadedIntoScene to true again.
    PlayerNetworkStartHook.exitMessageReceieved = true;
  }

}