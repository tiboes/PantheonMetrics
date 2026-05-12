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


//[HarmonyPatch(typeof(UIChatWindows), nameof(UIChatWindows.PassMessage), typeof(string), typeof(string), typeof(ChatChannelType))]
public class ChatWindowsHook
{
  private static void Prefix(UIChatWindows __instance, string name, string message, ChatChannelType channel)
  {
    if (__instance == null)
    {
      MetricsLogging.LogMessageToConsole($"[ChatWindowsHook] Instance is null {name} - {message} - {channel}");
      return;
    }

    if (channel == ChatChannelType.PlayerSay && message.StartsWith("MetricsCommand"))
    {
      MetricsLogging.LogMessageToConsole($"[PotentialHooks] [{name}] - [{message}] - [{channel}]");
      message = "Hi";
    }
    
  }
}


//[HarmonyPatch(typeof(UIChat), nameof(UIChat.AddMessage))]
public class PotentialHooks
{

  private static void Prefix(UIChat __instance, 
    string name, 
    string message, 
    ChatChannelType channel, 
    CombatLogDirectionalFilter directionalFilter = CombatLogDirectionalFilter.All, 
    CombatLogFilter combatLogFilter = CombatLogFilter.Both, 
    CombatLogPlayerFilter combatLogPlayerFilter = CombatLogPlayerFilter.All, 
    bool processForChannel = true, 
    bool handleInactivity = true)
  {
    if (__instance == null)
    {
      MetricsLogging.LogMessageToConsole($"[PotentialHooks] Instance is null {name} - {message} - {channel}");
      return; 
    }

    var lastMsg = __instance.LastMessage;
    MetricsLogging.LogMessageToConsole($"[PotentialHooks] LastMessage: {lastMsg}:  {name} - {message} - {channel}");

    if (channel == ChatChannelType.PlayerSay && message.StartsWith("MetricsCommand"))
    {
      MetricsLogging.LogMessageToConsole($"[PotentialHooks] {name} - {message} - {channel}");
      message = "Hi"; 
    }


    //if (channel != ChatChannelType.Warning || !message.StartsWith("/metrics"))
    //{
    //  MetricsLogging.LogMessageToConsole($"[PotentialHooks] not warning or not /metrics:  {name} - {message} - {channel}");
    //  return; 
    //}

    //MetricsLogging.LogMessageToConsole($"[PotentialHooks] {name} - {message} - {channel}");

    //message = "Message Caught";

    
  }



  /***
  Initial Server Detection (UIRealmListItem.Init):
Hooks into server list initialization
Checks toggle.isOn to detect pre-selected servers
Stores this as preselectedServerName

User Selection (UIRealmListItem.OnSelected and UIRealmListItem.Select):
Captures both manual clicks and programmatic selections
Gets server name from __instance.Name
Stores this as activelySelectedServerName

Display Management (ModMain.SetServerName):
Maintains two states: preselectedServerName and activelySelectedServerName
Uses activelySelectedServerName if available, falls back to preselectedServerName
Updates a TextMeshProUGUI component positioned below the compass

The display is created when the compass panel loads (UIWindowPanel.Start), and the text updates automatically whenever the server selection changes.



  Input.GetKeyDown




   */
}
