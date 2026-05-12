using Il2Cpp;
using PantheonMetrics.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsCombat
{
  private static int maxRecordKeepingTimeInSeconds = 10;
  private static IEntity? LatestAttacker { get; set; } = null;
  private static IEntity? LatestDefender { get; set; } = null;
  private static DateTime LastResetEncounterTime { get; set; } = DateTime.MinValue;

  private static List<DamageInstanceObject> DamageInstancesDPS { get; set; } = new List<DamageInstanceObject>();
  private static List<DamageInstanceObject> EncounterDamageInstances { get; set; } = new List<DamageInstanceObject>();
  private static List<DamageInstanceObject> LastEncounterDamageInstancesSaved { get; set; } = new List<DamageInstanceObject>();


  public static DateTime? CombatStartTime { get; set; } = null;
  public static DateTime? CombatEndTime { get; set; } = null;

  public static bool ShowAttackerBreakdown { get; set; } = true;
  public static bool ShowAbilityBreakdown { get; set; } = true;
  public static bool ShowDamageTypeBreakdown { get; set; } = true;
  public static bool ShowMitigatedDamage { get; set; } = false;

  public static bool ShowEncounterInChat { get; set; } = true;

  public static List<string> LatestEncounterLinesSaved { get; set; } = new List<string>();


  public static void AddDamageInstance(DateTime time, string attackerName, string defenderName, int damage, float mitigatedDamage, string damageAbility, string damageType, string result, string direction)
  {
    if (result == "Miss" || result == "Resist")
      return;
    //if (result != "Hit" || result != "Hit, Crit")
    //  MetricsLogging.LogMessageToConsole($"{damage} - {mitigatedDamage} - {damageAbility} - {damageType} - {result}");

    DateTime timecap = DateTime.Now.AddSeconds(-maxRecordKeepingTimeInSeconds);
    int removedDamageInstances = DamageInstancesDPS.RemoveAll(d => d.Time < timecap);

    var instance = new DamageInstanceObject(time, attackerName, defenderName, damage, mitigatedDamage, damageAbility, damageType, result);

    DamageInstancesDPS.Add(instance);
    EncounterDamageInstances.Add(instance);
  }

  public static int GetEncounterDurationInSeconds(List<DamageInstanceObject> instances) => instances.Any() ? (int)(instances.Last().Time - instances.First().Time).TotalSeconds  : 0;
  public static double GetEncounterDpsSimple()
  {
    if(!EncounterDamageInstances.Any())
      return 0;
    var seconds = GetEncounterDurationInSeconds(EncounterDamageInstances);
    var damage = EncounterDamageInstances.Sum(e => e.Damage);
    if (seconds < 1)
      return damage;
    return damage / seconds;

  }
  public static string GetEncounterDuration(List<DamageInstanceObject> instances)
  {
    var seconds = GetEncounterDurationInSeconds(instances);
    if (seconds < 60)
      return $"00:{seconds:00}";

    var minutes = (int)(seconds / 60);
    var leftOverSeconds = seconds - (minutes * 60);
    return $"{minutes:00}:{leftOverSeconds:00}";
  }

  public static string GetEncounterDps()
  {
    if (!EncounterDamageInstances.Any())
      return "0";

    var seconds = GetEncounterDurationInSeconds(EncounterDamageInstances);
    
    var byAttacker = EncounterDamageInstances.GroupBy(e => e.AttackerName);
    if (byAttacker.Count() == 1)
      return $"{(EncounterDamageInstances.Sum(e => e.Damage) / seconds):#.#}";

    string output = $"{((EncounterDamageInstances.Sum(e => e.Damage) / seconds)):#.#}: ";
    foreach ( var attacker in byAttacker.ToList())
    {
      output += $"{attacker.Key}: {((attacker.Sum(e => e.Damage) / seconds)):#.#}, ";
    }
    output = output.Remove(output.Length - 2);
    return output ;
  }

  public static DateTime EncounterStart
  {
    get
    {
      if (!EncounterDamageInstances.Any())
        return DateTime.MinValue;
      return EncounterDamageInstances.First().Time;
    }
  }

  public static DateTime EncounterEnd
  {
    get
    {
      if (!EncounterDamageInstances.Any())
        return DateTime.MinValue;
      return EncounterDamageInstances.Last().Time;
    }
  }

  public static void ResetEncounter()
  {
    CombatStartTime = null;
    CombatEndTime = null;
    EncounterDamageInstances = new List<DamageInstanceObject>();
    LastResetEncounterTime = DateTime.Now;
    //ResetDpsMeter();
  }

  public static void ResetDpsMeter() 
  { 
    DamageInstancesDPS = new List<DamageInstanceObject>();
  }

  public static double DamagePerSecond()
  {
    //maxRecordKeepingTimeInSeconds
    var instances = DamageInstancesDPS.Where(d => d.Time >= DateTime.Now.AddSeconds(-maxRecordKeepingTimeInSeconds)).ToList();
    if (!instances.Any())
      return 0;
    //if (instances.Count == 1)
    //  return instances.Sum(d => d.Damage);

    var seconds = ((instances.Last().Time - instances.First().Time).TotalSeconds);

    if (seconds < 1)
      return instances.Sum(d => d.Damage);

    return (instances.Sum(d => d.Damage) / seconds);
  }


  public static List<string> GetEncounterResultAsLines()
  {
    var encounterDamageInstances = EncounterDamageInstances;
    if (!EncounterDamageInstances.Any() || LastResetEncounterTime > DateTime.Now.AddSeconds(-1))
      encounterDamageInstances = LastEncounterDamageInstancesSaved;
    if (!encounterDamageInstances.Any())
      return new List<string>();

    List<string> result = new List<string>();
    var seconds = GetEncounterDurationInSeconds(encounterDamageInstances);
    if (seconds < 1)
      seconds = 1;

    var totalDamageDealt = encounterDamageInstances.Sum(e => e.Damage);
    var totalMitigationDealt = encounterDamageInstances.Sum(e => e.MitigatedDamage);

    var start = encounterDamageInstances.First().Time;
    var end = encounterDamageInstances.Last().Time;

    result.Add($"Encounter Duration: {start:HH:mm:ss} - {end:HH:mm:ss}  ({GetEncounterDuration(encounterDamageInstances)})");
    string mitigatedEncounterText = ShowMitigatedDamage ? $"- Mitigation {(totalMitigationDealt):#} " : "";
    result.Add($"Encounter Damage: {(totalDamageDealt):#} {mitigatedEncounterText}- DPS: {(totalDamageDealt / seconds):#}");



    int dpsLength = 10;
    int damageLength = 10;
    int mitigationLength = 10;
    int nameLength = encounterDamageInstances.Max(m => m.AttackerName.Length);
    nameLength = nameLength < 20 ? 20 : nameLength;

    string migitagedSubHeader = ShowMitigatedDamage ? " | Mitigated" : "";
    int noParticipants = 1;

    if (ShowAttackerBreakdown)
    {
      result.Add($"");
      result.Add($"Breakdown by participant");
      OutputByString(result, encounterDamageInstances.GroupBy(e => e.AttackerName), totalDamageDealt);
    }

    if (ShowAbilityBreakdown)
    {
      result.Add($"");
      result.Add($"Breakdown by skill - Damage{migitagedSubHeader}");
      OutputByString(result, encounterDamageInstances.GroupBy(e => e.DamageAbility), totalDamageDealt);
    }
    if (ShowDamageTypeBreakdown)
    {
      result.Add($"");
      result.Add($"Breakdown by DamageType - Damage{migitagedSubHeader}");
      OutputByString(result, encounterDamageInstances.GroupBy(e => e.DamageType), totalDamageDealt);
    }
    LastEncounterDamageInstancesSaved = encounterDamageInstances;
    return result;
  }
    
  public static (string Outstring, int lines) GetEncounterResult() 
  {
    if (!EncounterDamageInstances.Any())
      return ("",0);

    if (!ShowEncounterInChat)
      return ("", 0);

    StringBuilder result = new StringBuilder();


    var seconds = GetEncounterDurationInSeconds(EncounterDamageInstances);
    if (seconds < 1)
      seconds = 1;

    var totalDamageDealt = EncounterDamageInstances.Sum(e=> e.Damage);
    var totalMitigationDealt = EncounterDamageInstances.Sum(e => e.MitigatedDamage);

    var start = EncounterStart;
    var end = EncounterEnd;

    result.AppendLine($"Encounter Duration: {start:HH:mm:ss} - {end:HH:mm:ss}  ({GetEncounterDuration(EncounterDamageInstances)})");
    string mitigatedEncounterText = ShowMitigatedDamage ? $"- Mitigation {(totalMitigationDealt):#} " : "";
    result.AppendLine($"Encounter Damage: {(totalDamageDealt):#} {mitigatedEncounterText}- DPS: {(totalDamageDealt / seconds):#}");



    int dpsLength = 10;
    int damageLength = 10;
    int mitigationLength = 10;
    int nameLength = EncounterDamageInstances.Max(m => m.AttackerName.Length);
    nameLength = nameLength < 20 ? 20 : nameLength;

    string migitagedSubHeader = ShowMitigatedDamage ? " | Mitigated" : "";
    int noParticipants = 1;

    if (ShowAttackerBreakdown)
    {
      result.AppendLine($"");
      result.AppendLine($"Breakdown by participant");
      OutputByString(result, EncounterDamageInstances.GroupBy(e => e.AttackerName), totalDamageDealt);
    } 

    if (ShowAbilityBreakdown)
    {
      result.AppendLine($"");
      result.AppendLine($"Breakdown by skill - Damage{migitagedSubHeader}");
      OutputByString(result, EncounterDamageInstances.GroupBy(e => e.DamageAbility), totalDamageDealt);
    }
    if (ShowDamageTypeBreakdown)
    {
      result.AppendLine($"");
      result.AppendLine($"Breakdown by DamageType - Damage{migitagedSubHeader}");
      OutputByString(result, EncounterDamageInstances.GroupBy(e => e.DamageType), totalDamageDealt);
    }
    
    return (result.ToString(), (2 + noParticipants));
  }

  private static void OutputByString(StringBuilder result, IEnumerable<IGrouping<string, DamageInstanceObject>> grouping, int totalDamage)
  {
    List<(decimal percentage, string outputString)> percentages = new List<(decimal percentage, string outputString)> ();
    foreach (var item in grouping)
    {
      var damage = item.Sum(i => i.Damage);
      decimal percentage = (decimal)damage / totalDamage;
      var outputText = $"({percentage:P1}) - {GetByOutputString(item)}";
      percentages.Add((percentage, outputText));
    }
    ;
    foreach (var item in percentages.OrderByDescending(x => x.percentage)) 
    {
      result.AppendLine(item.outputString);
    }
  }
  private static void OutputByString(List<string> result, IEnumerable<IGrouping<string, DamageInstanceObject>> grouping, int totalDamage)
  {
    List<(decimal percentage, string outputString)> percentages = new List<(decimal percentage, string outputString)>();
    foreach (var item in grouping)
    {
      var damage = item.Sum(i => i.Damage);
      decimal percentage = (decimal)damage / totalDamage;
      var outputText = $"({percentage:P1}) - {GetByOutputString(item)}";
      percentages.Add((percentage, outputText));
    }
    ;
    foreach (var item in percentages.OrderByDescending(x => x.percentage))
    {
      result.Add(item.outputString);
    }
  }

  private static string GetByOutputString(String text, int damage, float mitigated, int count)
  {
    if (mitigated < 1)
      mitigated = 0;

    string mitTotal = "";
    string mitAvg = "";
    if (ShowMitigatedDamage)
    {
      mitTotal = mitigated == 0 ? " | 0" : $" | {mitigated:#}";
      mitAvg = mitigated == 0 ? " | 0" : $" | {(mitigated / count):#}";
    }
    if (count > 1)
      return string.Format("{0}({1}) Total({2}{3}), Average({4}{5})", text, count, $"{damage:#}", mitTotal, $"{(damage / count):#}", mitAvg);

    return string.Format("{0}({1}) Total({2}{3})", text, count, $"{damage:#}", mitTotal);
  }

  private static string GetByOutputString(IGrouping<string, DamageInstanceObject> grouping)
  {
    return GetByOutputString(grouping.Key, grouping.Sum(s => s.Damage), grouping.Sum(s => s.MitigatedDamage), grouping.Count());
  }
}
