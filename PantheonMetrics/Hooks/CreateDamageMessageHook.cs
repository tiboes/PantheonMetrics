using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPantheonPersist;
using MelonLoader;
using PantheonMetrics.Data;
using UnityEngine.Playables;
using System.Linq;
using Il2CppLogicalGraphNodes;

namespace PantheonMetrics.Hooks;

//[HarmonyPatch(typeof(UIChatWindows), nameof(UIChatWindows.CreateDamageMessage), typeof(string), typeof(int), typeof(string), typeof(string), typeof(CombatResultType), typeof(DamageType), typeof(CombatLogDirectionalFilter), typeof(CombatLogFilter), typeof(bool), typeof(float))]
[HarmonyPatch(typeof(UIChatWindows), nameof(UIChatWindows.CreateDamageMessage))]
public  class CreateDamageMessageHook
{

  //private static void Postfix(UIChatWindows __instance, string attackerName, int damage, string defenderName, string abilityOrBuffName, CombatResultType combatResultType, DamageType damageType, CombatLogDirectionalFilter direction, CombatLogFilter filter, bool ignoreAttackerName, float mitigatedDamage)//, out ChatChannelType channel){
  //{
  //  if (defenderName == MetricsPlayer.PlayerName)
  //  {
  //    MetricsLogging.LogMessageToConsole($"PostFix, Damage taken from {attackerName}: Damage: {damage}, Mitigated: {mitigatedDamage}, Health Updates since last scan (Before/now): {MetricsPlayer.OldHealth}/{MetricsPlayer.CurrentHealth}");
  //  }


  //  if (__instance == null)
  //    return;

  //  MetricsLogging.LogMessageToConsole($"CreateDamageMessageHook.PostFix.");

  //}

  private static void Prefix(UIChatWindows __instance, string attackerName, int damage, string defenderName, string abilityOrBuffName, CombatResultType combatResultType, DamageType damageType, CombatLogDirectionalFilter direction, CombatLogFilter filter, bool ignoreAttackerName, float mitigatedDamage)//, out ChatChannelType channel)
  {
    //MetricsLogging.LogMessageToConsole($"[CreateDamageMessage] - {attackerName} -> {defenderName}: Damage: {damage}, Mitigated: {mitigatedDamage} - {combatResultType}|{direction}|{filter}|{ignoreAttackerName}");





    if (defenderName == MetricsPlayer.PlayerName) 
    {
      
      //MetricsLogging.LogMessageToConsole($"Damage taken from {attackerName}: Damage: {damage}, Mitigated: {mitigatedDamage}, Health Updates since last scan (Before/now): {MetricsPlayer.OldHealth}/{MetricsPlayer.CurrentHealth}");
      //MetricsLogging.LogMessageToConsole($"Breath: {MetricsPlayer.CurrentBreath}");
      //UIChatWindows.Instance.PassMessage(message, ChatChannelType.Info);
    }

    if ( attackerName == MetricsPlayer.PlayerName || MetricsPlayer.Pets.ContainsKey(attackerName) )
      MetricsCombat.AddDamageInstance(DateTime.Now, attackerName, defenderName,damage, mitigatedDamage, abilityOrBuffName, damageType.AsString(), combatResultType.ToString());

/***
    if (MetricsPlayer.Pets.ContainsKey(attackerName))
    {
      MetricsLogging.LogMessageToConsole($"[CreateDamageMessageHook] Pet Attack: A: {attackerName}, D: {defenderName}, DMG: {damage}, Mit DMG: {mitigatedDamage}, ability: {abilityOrBuffName}, combat result: {combatResultType}, DMG type: {damageType}, direction: {direction}, filter: {filter}, ignoreAttackerName: {ignoreAttackerName}");
    }
    else if (attackerName == MetricsPlayer.PlayerName)
    {
      MetricsLogging.LogMessageToConsole($"[CreateDamageMessageHook] Player Attack: A: {attackerName}, D: {defenderName}, DMG: {damage}, Mit DMG: {mitigatedDamage}, ability: {abilityOrBuffName}, combat result: {combatResultType}, DMG type: {damageType}, direction: {direction}, filter: {filter}, ignoreAttackerName: {ignoreAttackerName}");
    }
    else
    {
      //Could be group, non group player/pet or monster
      //MetricsLogging.LogMessageToConsole($"[CreateDamageMessageHook] Not Player Attack: A: {attackerName}, D: {defenderName}, DMG: {damage}, Mit DMG: {mitigatedDamage}, ability: {abilityOrBuffName}, combat result: {combatResultType}, DMG type: {damageType}, direction: {direction}, filter: {filter}, ignoreAttackerName: {ignoreAttackerName}");
    }
*/
      //MetricsBookKeeper.AccumulatedDamage += damage;
      //MetricsBookKeeper.AccumulatedMitigatedDamage += mitigatedDamage;

      //MetricsLogging.LogMessageToConsole($"Damage dealt to  {defenderName}: Damage: {damage}, Mitigated: {mitigatedDamage}, Health Updates since last scan (Before/now): {MetricsPlayer.OldHealth}/{MetricsPlayer.CurrentHealth}");

      // MetricsLogging.LogMessageToConsole($"CreateDamageMessageHook.Prefix.0.{attackerName},{damage},{defenderName},{abilityOrBuffName},{combatResultType},{damageType},{direction},{filter},{ignoreAttackerName},{mitigatedDamage}");

      //MetricsLogging.LogMessageToConsole($"AccumulatedDamage: {MetricsBookKeeper.AccumulatedDamage}, AccumulatedMitigatedDamage: {MetricsBookKeeper.AccumulatedMitigatedDamage}");

      //Stats.Logic v = MetricsPlayer.PlayerGameObject.Stats;

      //var health = v.GetStat(StatType.Health);


      //var e = v.experience;
      //var level = e.Level;
      //var st = v.stats;
      ////foreach (Stat? item in st)
      ////{
      ////  if (item != null)
      ////  {
      ////    MetricsLogging.LogMessageToConsole($"  StatType: {item.StatType}, Value: {item.Value}");
      ////  }
      ////}

      //try
      //{
      //  Group.Logic group = MetricsPlayer.PlayerGameObject.Group;
      //  if (group != null) 
      //  {
      //    ActiveGroup ag = group.Current;
      //    if (ag != null)
      //    {
      //      foreach (GroupMember? item in ag.members)
      //      {

      //        if (item != null)
      //        {
      //          MetricsLogging.LogMessageToConsole($"  GroupMember: {item.Name}[{item.Class}], IsLeader: {item.IsLeader}");
      //        }
      //      }
      //    } 
      //    else
      //    {
      //      MetricsLogging.LogMessageToConsole($"  Group.Logic group.Current is null");
      //    }
      //  }
      //  else
      //  {
      //    MetricsLogging.LogMessageToConsole($"  Group.Logic group = MetricsPlayer.PlayerGameObject.Group; is null");
      //  }


      //}
      //catch (Exception ex) 
      //{ 
      //  MetricsLogging.LogMessageToConsole($"  Exception while accessing group info: {ex.Message}");
      //}








    //}
  }
}
