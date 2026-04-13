using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PantheonMetrics.Data;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.Logics;

public static  class WaterManagement
{
  private static readonly int breathThresholdWarningLevel1 = 80;
  private static readonly int breathThresholdWarningLevel2 = 30;
  private static bool breathWarningGivenForLevel1 = false;
  private static bool breathWarningGivenForLevel2 = false;

  public static void WaterBadlevel1()
  {
    GUI.Label(new Rect(200, 200, 1000, 200), $"<b><color=Yellow><size=100>WATER..BAD</size></color></b>");
  }
  public static void WaterBadlevel2()
  {
    GUI.Label(new Rect(200, 200, 1000, 200), $"<b><color=Red><size=100>WATER..BAD</size></color></b>");
  }

  public static void HandleWaterLogic(EntityStatusType status, bool hasStatus)
  {
    return;
    if (!MetricsConfiguration.BreathWarningEnabled || status != EntityStatusType.Submerged)
      return;

    MetricsLogging.LogMessageToConsole($"[WaterManagement.HandleWaterLogic] IN WATER HANDLING");

    if (status == EntityStatusType.Submerged && hasStatus)
    {
      var currentBreath = MetricsPlayer.CurrentBreath;
      if (currentBreath < breathThresholdWarningLevel1 && !breathWarningGivenForLevel1)
      {
        MetricsLogging.LogMessageToConsole($"[WaterManagement.HandleWaterLogic] Your Breath has fallen below the Threshold {breathThresholdWarningLevel1}%");
        breathWarningGivenForLevel1 = true;
        MelonEvents.OnGUI.Subscribe(WaterBadlevel1, 100); // Register the 'Frozen' label
      }
      if (currentBreath < breathThresholdWarningLevel2 && !breathWarningGivenForLevel2)
      {
        MetricsLogging.LogMessageToConsole($"[WaterManagement.HandleWaterLogic] Your Breath has fallen below the Threshold {breathThresholdWarningLevel2}%");
        breathWarningGivenForLevel2 = true;
        MelonEvents.OnGUI.Unsubscribe(WaterBadlevel1); // Unregister the 'Frozen' label
        MelonEvents.OnGUI.Subscribe(WaterBadlevel2, 100); // Register the 'Frozen' label

      }
    }
    if (status == EntityStatusType.Submerged && !hasStatus && breathWarningGivenForLevel1)
    {
      var currentBreath = MetricsPlayer.CurrentBreath;
      if (currentBreath > breathThresholdWarningLevel2 && breathWarningGivenForLevel2) 
      {
        MetricsLogging.LogMessageToConsole($"[WaterManagement.HandleWaterLogic] Your Breath has risen above the Threshold {breathThresholdWarningLevel2}%");
        breathWarningGivenForLevel2 = false;
        MelonEvents.OnGUI.Unsubscribe(WaterBadlevel2); // Unregister the 'Frozen' label
      }
      if (currentBreath > breathThresholdWarningLevel1 && breathWarningGivenForLevel1)
      {
        MetricsLogging.LogMessageToConsole($"[WaterManagement.HandleWaterLogic] Your Breath has risen above the Threshold {breathThresholdWarningLevel1}%");
        breathWarningGivenForLevel1 = false;
        MelonEvents.OnGUI.Unsubscribe(WaterBadlevel1); // Unregister the 'Frozen' label

      }

    }

  }
}
