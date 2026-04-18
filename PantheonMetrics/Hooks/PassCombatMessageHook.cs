using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.Hooks;

//[HarmonyPatch(typeof(UIChatWindow), nameof(UIChatWindow.PassCombatMessage))]
public class PassCombatMessageHook
{
  private static long _lastMessageTime = 0;
  private static readonly long MaxAllowedTicksBetweenSimilarMessages = TimeSpan.TicksPerSecond/10;
  private static int _hashOfLastMessage = 0;


  private static void Postfix(UIChatWindow __instance, IEntity attacker, IEntity defender, bool isDamage, string name, string message, ChatChannelType channel, CombatLogDirectionalFilter direction, CombatLogFilter filter, CombatLogPlayerFilter playerFilter)
  {
    //NOTE THAT When someone (or a pet) dies there can be a message where the attacker is null
    //Since im not using any of this code right now it is disabled
    //we can probably bypass this by having a static attacker/defender variable being set in CreateCombatMessage, and checking thisone against it

    if (attacker == null)
    {
      MetricsLogging.LogMessageToConsole($"[PassCombatMessageHook] Attacker is null, message: {message}, defender: {defender==null}");
    }
    else
    {
     // MetricsCombat.LatestAttacker = attacker;
    }



    if (defender == null)
    {
      MetricsLogging.LogMessageToConsole($"[PassCombatMessageHook] defender is null, message: {message}, attacker: {attacker == null}");
    }
    else
    {
      //MetricsCombat.LatestDefender = defender;
    }


    var attackerNetworkId = attacker?.NetworkId;
    var defenderNetworkId = defender?.NetworkId;

    //Avoid duplicate messages
    if (IsDuplicateMessage(message, attackerNetworkId, defenderNetworkId))
      return;

    if (isDamage)
    {
      MetricsLogging.LogMessageToConsole($"[PassCombatMessageHook] ATK: {attacker?.Info.DisplayName}, DEF: {defender?.Info.DisplayName}, isDMG: {isDamage}, channel: {channel}, direction: {direction}, filter: {filter}, playerFilter: {playerFilter}, message: {message}");
    }
    


    var attackerDisplayName = attacker?.Info.DisplayName;
    var defenderDisplayName = defender?.Info.DisplayName;


    //MetricsLogging.LogMessageToConsole($"[PassCombatMessage] - {attackerDisplayName} -> {defenderDisplayName} ({attackerNetworkId} -> {defenderNetworkId}) - {isDamage}|{direction}|{filter}|{playerFilter}");




    //Positive or negative?
    //MetricsLogging.LogMessageToConsole($"[Attacker/Defender: {attackerDisplayName}/{defenderDisplayName}] - IsDamage[{isDamage} - {filter}] {channel}({direction}) - {playerFilter}  - Message: {message}");

  }

  private static bool IsDuplicateMessage(string message, Il2CppViNL.NetworkId? attackerNetworkId, Il2CppViNL.NetworkId? defenderNetworkId)
  {
    try
    {
      var currentMessageTime = DateTime.UtcNow.Ticks;
      if (_lastMessageTime == 0)
        _lastMessageTime = currentMessageTime;

      var timeSinceLastMessage = currentMessageTime - _lastMessageTime;
      var currentMessageHash = $"{attackerNetworkId?.Value}-{defenderNetworkId?.Value}-{message}".GetHashCode();


      var result = timeSinceLastMessage != 0 && currentMessageHash == _hashOfLastMessage && timeSinceLastMessage < MaxAllowedTicksBetweenSimilarMessages;
      _lastMessageTime = currentMessageTime;//make sure to update the last time
      _hashOfLastMessage = currentMessageHash;
      return result;

    } catch (Exception ex) 
    {
      MetricsLogging.LogMessageToConsole($"[PassCombatMessageHook] Exception in DuplicateMessage");
      throw;
      
    }

    
  }
}
