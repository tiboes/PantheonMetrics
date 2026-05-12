using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsConfiguration
{
  public static bool BreathWarningEnabled { get; set; } = true;
  public static bool ExperienceMetricEnabled { get; set; } = true;
  public static bool CombatTrackingEnabled { get; set; } = true;


  public static bool HandleSlashCommand(string message)
  {
    if (!MetricsPlayer.IsPlayerLoadedIntoScene)
      return true;

      var msg = message.ToLower();
    if (!msg.StartsWith("/metrics") )
      return true;


    msg = msg.Substring("/metrics".Length).Trim();
    if (msg=="")
      PantheonMetricsMain.ShowMetrics();

    
    if (msg.StartsWith("loc remove"))
    {
      var idString = msg.Substring("loc remove".Length).Trim();
      var couldParse = int.TryParse(idString, out int id);
      if (couldParse)
        MetricsLocation.RemoveLocation(id);

      return false;
    }
    if (msg.StartsWith("loc reset"))
    {
      MetricsLocation.ResetLocations();
    }


    if (msg.StartsWith("loc"))
    {
      var loc = msg.Substring("loc".Length).Trim();
      var split = loc.Split(' ');
      if (split.Length == 2)
      {
        var xCoodParsed = int.TryParse(split[0], out int x);
        var yCoodParsed = int.TryParse(split[1], out int y);
        if (!(xCoodParsed && yCoodParsed))
          return false;

        MetricsLocation.AddLocation(x, y, $"X: {x}, Y: {y}");
      }
      if (split.Length > 2) 
      {
        var xCoodParsed = int.TryParse(split[0], out int x);
        var yCoodParsed = int.TryParse(split[1], out int y);
        if (!(xCoodParsed && yCoodParsed))
          return false;

        string text = $"X: {x}, Y: {y}";
        for (int i = 2; i < split.Length; i++) 
          text += $" {split[i]}";

        MetricsLocation.AddLocation(x, y, text);
      }


      return false;
    }
    if (msg.StartsWith("enc"))
    {
      var toggle = msg.Substring("enc".Length).Trim();
      switch (toggle)
      {
        case "mitigated":
          MetricsCombat.ShowMitigatedDamage = !MetricsCombat.ShowMitigatedDamage;
          break;
        case "attacker":
          MetricsCombat.ShowAttackerBreakdown = !MetricsCombat.ShowAttackerBreakdown;
          break;
        case "ability":
          MetricsCombat.ShowAbilityBreakdown = !MetricsCombat.ShowAbilityBreakdown;
          break;
        case "type":
          MetricsCombat.ShowDamageTypeBreakdown = !MetricsCombat.ShowDamageTypeBreakdown;
          break;
      }
      return false;
    }


    return false;
  }

}
