using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
using UnityEngine;

namespace PantheonMetrics.Hooks;


[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetOffensive))]
public class TargetLogicHook
{
  private static void Postfix(Targets.Logic __instance)
  {
    if (__instance == null)
      return;

    var off = __instance.Offensive;
    if (off == null)
      return;//TODO change when also looking at defensive target



    //var nameplate = off.Nameplate;
    //if (nameplate == null)
    //  MetricsLogging.LogMessageToConsole($"Nameplate is null");

    //var displayname = off.Info?.DisplayName;
    //if (displayname != null)
    //  MetricsLogging.LogMessageToConsole($"Offensive target name: {displayname}, id: {off.NetworkId.Value}");

    //if (off.Pools != null)
    //{
    //  var maxHealth = off.Pools.GetMax(PoolType.Health);
    //  var currentHealth = off.Pools.GetCurrent(PoolType.Health);
    //  MetricsLogging.LogMessageToConsole($"TargetSetOffensiveHook. MaxHealth: {maxHealth}, CurrentHealth: {currentHealth}");
    //}

  }
}