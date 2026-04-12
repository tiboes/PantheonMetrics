using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
using UnityEngine;


namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(Experience.Logic), nameof(Experience.Logic.SetExperience))]
public class ExperienceHook
{
  private static void Postfix(Experience.Logic __instance, int experience, bool levelUpEvent)
  {
    if (!MetricsPlayer.IsPlayerLoadedIntoScene)
      return;
    //if (levelUpEvent)
    //  MetricsExperience.CullOlderMessages(0);

    var killedMonster = MetricsExperience.AddExperience(experience);



    if (__instance != null)
    {
      int totalExperience = __instance.Total;
      int experienceToReachNextLevel = __instance.experienceToReachNextLevel;
      int experienceToReachCurrentLevel = __instance.experienceToReachCurrentLevel;
      //MetricsLogging.LogMessageToConsole($"{experience}, {levelUpEvent}, {totalExperience}, {experienceToReachNextLevel}, {experienceToReachCurrentLevel}");


    }
    if (killedMonster != null)
    {
      MetricsLogging.LogMessageToInfoChat($"Killed {killedMonster}({MetricsExperience.GetLastExperienceGain()}) - Exp pr. min: {MetricsExperience.GetExperiencePerMin10MinSliding()}");
      MetricsLogging.LogMessageToConsole($"Killed {killedMonster.ToStringDebug()} - Exp {MetricsExperience.GetLastExperienceGain()} - Exp Last Hour Total/Min:{MetricsExperience.GetExperienceTheLast10Mins()}/{MetricsExperience.GetExperiencePerMin10MinSliding()}");
    }
  }
}
