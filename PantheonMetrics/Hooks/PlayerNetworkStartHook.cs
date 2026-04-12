using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PantheonMetrics.Data;

namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStart))]
public class PlayerNetworkStartHook
{
  private static void Prefix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    if (__instance.NetworkId.Value == 1)
    {
      return;
    }

    if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
    {
      //MetricsBookKeeper.Counter++;
      //MetricsLogging.LogMessageToConsole($"PlayerNetworkStartHook.Prefix. Counter: {MetricsBookKeeper.Counter}");
    }
  }

  private static void Postfix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    if (__instance.NetworkId.Value == 1)
    {
      return;
    }

    if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
    {
      MetricsPlayer.PlayerName = __instance.info.DisplayName;
      MetricsPlayer.PlayerGameObject = __instance;
      var initialHealth = MetricsPlayer.CurrentHealth;
      uint playerNetworkId = __instance.NetworkId.Value;

      MetricsExperience.ResetExperience(__instance.Experience.Total);//TODO does this reset when switching zones?




      MetricsLogging.LogMessageToConsole($"Loading in as [{MetricsPlayer.PlayerName}({MetricsPlayer.PlayerNetworkId})]. Total Health: {initialHealth}");
      MetricsPlayer.IsPlayerLoadedIntoScene = true;
    }

  }
}

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStop))]
public class PlayerNetworkStopHook
{
  //private static void Prefix(EntityPlayerGameObject __instance)
  //{
  //  // Fired in character select
  //  if (__instance.NetworkId.Value == 1)
  //  {
  //    return;
  //  }

  //  if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
  //  {
  //    //MetricsBookKeeper.Counter++;
  //    //MetricsLogging.LogMessageToConsole($"PlayerNetworkStartHook.Prefix. Counter: {MetricsBookKeeper.Counter}");
  //  }
  //}

  private static void Postfix(EntityPlayerGameObject __instance)
  {
    // Fired in character select
    if (__instance.NetworkId.Value == 1)
    {
      return;
    }

    MetricsLogging.LogMessageToConsole($"Logging Character {MetricsPlayer.PlayerName}");

    //Do clean up
    MetricsPlayer.PlayerGameObject = null;
    MetricsPlayer.IsPlayerLoadedIntoScene = false;
    MetricsExperience.ResetExperience(0);

    

    //if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
    //{
    //  MetricsPlayer.PlayerName = __instance.info.DisplayName;
    //  MetricsPlayer.PlayerGameObject = __instance;
    //  var initialHealth = MetricsPlayer.CurrentHealth;
    //  uint playerNetworkId = __instance.NetworkId.Value;

    //  MetricsExperience.ResetExperience(__instance.Experience.Total);//TODO does this reset when switching zones?




    //  MetricsLogging.LogMessageToConsole($"Loading in as [{MetricsPlayer.PlayerName}({MetricsPlayer.PlayerNetworkId})]. Total Health: {initialHealth}");
    //  MetricsPlayer.IsPlayerLoadedIntoScene = true;
    //}

  }

}