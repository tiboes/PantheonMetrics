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

  private static void ResetExperience()
  {
    lastRegisteredTotalExperience = 0;
    experienceEarned = new List<ExperienceObject>();
  }

  public static void ResetExperience(int currentExperience)
  {
    lastRegisteredTotalExperience = currentExperience;
    experienceEarned = new List<ExperienceObject>();
  }


  public static EntityObject? AddExperience(int experience)
  {
    var last = LastRegisteredDeath;
    if (last == null)
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
    
  }

  public static EntityObject? LastRegisteredDeath { get; set; } = null;

  public static int GetLastRegisteredTotalExperience() => lastRegisteredTotalExperience;
  public static int GetLastExperienceGain() => lastRegisteredExperience;
  
  public static int GetExperienceSince(DateTime time) => experienceEarned.Where(e => e.EarnedTime >= time).Sum(y=> y.Experience);

  public static int GetExperienceTheLast10Mins() => GetExperienceSinceMinutes(10);
  //public static int GetExperienceTheLastFiveMinutes() => GetExperienceSinceMinutes(5);

  //public static decimal GetExperiencePerMinuteOverLastFiveminutes() => GetExperienceTheLastFiveMinutes() / 5;

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

  public static void CullOlderMessages(int minutesToKeep)
  {
    experienceEarned = experienceEarned.Where(e => e.EarnedTime >= DateTime.UtcNow.AddMinutes(-minutesToKeep)).ToList();
  }



}
