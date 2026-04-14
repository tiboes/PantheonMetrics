using Il2Cpp;
using Il2CppPantheonPersist;
using PantheonMetrics.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.Data;

public static class MetricsExperience
{
  
  private static List<ExperienceObject> experienceEarned = new List<ExperienceObject>();
  private static int lastRegisteredTotalExperience = 0;
  private static int lastRegisteredExperience = 0;
  public static List<(DateTime time, string enemy, int experience)> LastKills { get; set; } = new List<(DateTime time, string enemy, int experience)>();


  public static void ResetExperience()
  {
    if (MetricsPlayer.PlayerGameObject == null || MetricsPlayer.PlayerGameObject.Experience == null)
      return;

    ResetExperience(MetricsPlayer.PlayerGameObject.Experience.Total);
  }

  public static void ResetExperience(int currentExperience)
  {
    //TODO mabye at some point we will need to reset at leveling or something. but today is not that day!
    lastRegisteredTotalExperience = currentExperience;
    lastRegisteredExperience = 0;
    experienceEarned.Clear();
    LastKills.Clear();
    LastRegisteredDeath = null;
    TotalExperienceTheLast10MinCached = 0;
    ExperiencePerMinTheLast10MinCached = 0;
  }

  public static EntityObject? AddExperience(int experience)
  {
    var last = LastRegisteredDeath;
    if (last == null || last.Relation != EntityRelationEnum.Monster)
      return null;
    

    AddExperience(experience, last);
    return last;
  }
  public static void AddExperience(int experience, EntityObject experienceSource)
  {
    if (lastRegisteredTotalExperience > experience)//Must be a level up
    {
      lastRegisteredTotalExperience = experience;
      experienceEarned.Add(new ExperienceObject(lastRegisteredTotalExperience, experienceSource));

    }
    else
    {
      lastRegisteredExperience = experience - lastRegisteredTotalExperience;
      lastRegisteredTotalExperience = experience;
      experienceEarned.Add(new ExperienceObject(lastRegisteredExperience, experienceSource));
    }
    TotalExperienceTheLast10MinCached = GetExperienceTheLast10Mins();
    ExperiencePerMinTheLast10MinCached = GetExperiencePerMin10MinSliding();

    LastKills.Add((DateTime.Now, experienceSource.DisplayName, lastRegisteredExperience));
    if (LastKills.Count > 5)
      LastKills.RemoveAt(0);//Make sure its only the last 5 entries in this list
  }

  public static EntityObject? LastRegisteredDeath { get; set; } = null;

  public static int GetLastRegisteredTotalExperience() => lastRegisteredTotalExperience;
  public static int GetLastExperienceGain() => lastRegisteredExperience;
  
  public static int GetExperienceSince(DateTime time) => experienceEarned.Where(e => e.EarnedTime >= time).Sum(y=> y.Experience);

  public static int GetExperienceTheLast10Mins() => GetExperienceSinceMinutes(10);

  public static int GetExperiencePerMin10MinSliding()
  {
    //if the earliest exp gain is before 10 mins we need to take that as the offset
    var now = DateTime.UtcNow;
    var relevantExps = experienceEarned.Where(e => e.EarnedTime >= now.AddMinutes(-10)).ToList();
    if(relevantExps.Count == 1)
    {
      return (int)(relevantExps.Sum(r => r.Experience));
    }

    if (relevantExps.Count == experienceEarned.Count)
    {
      var earliest = relevantExps.Min(x => x.EarnedTime);
      var diffInMinsFraction = (now - earliest).TotalMinutes;

      return (int)(relevantExps.Sum(r => r.Experience) / diffInMinsFraction);
    }

    var exp = GetExperienceTheLast10Mins() / 10;

    return exp;
  }

  public static int GetExperienceSinceMinutes(int minutes) => GetExperienceSince(DateTime.UtcNow.AddMinutes(-minutes));

  public static int TotalExperienceTheLast10MinCached { get; set; }
  public static int ExperiencePerMinTheLast10MinCached { get; set; }

  public static void CullOlderMessages(int minutesToKeep)
  {
    experienceEarned = experienceEarned.Where(e => e.EarnedTime >= DateTime.UtcNow.AddMinutes(-minutesToKeep)).ToList();
  }

}
