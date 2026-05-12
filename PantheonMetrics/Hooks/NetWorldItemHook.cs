using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPantheonPersist;
using MelonLoader;
using PantheonMetrics.Data;
using UnityEngine.Playables;
using System.Linq;
using Il2CppLogicalGraphNodes;
using Il2CppViNL;
using UnityEngine;

namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(NetworkWorldItem), nameof(NetworkWorldItem.NetworkStart))]
public class NetWorldItemHook
{
  private static void Prefix(NetworkWorldItem __instance, NetworkObject networkObject)
  {
    if (__instance == null)
      return;

    if (networkObject == null)
      return;

    var a = __instance.displayName.ToString();
    string b = __instance.worldItemType.ToString();
    var nameplate = __instance.worldItemType;

    //MetricsLogging.LogMessageToConsole($"[NetWorldItemHook] {a} - {b}");


  }
}
