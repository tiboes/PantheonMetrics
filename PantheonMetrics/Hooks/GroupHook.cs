using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
using UnityEngine;


namespace PantheonMetrics.Hooks;

[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.LeftGroup))]
public class LeftGroupHook
{

  private static void Postfix(Group.Logic __instance)
  {

    var updatedGroup = MetricsGroup.RefreshGroupMembers();

      //MetricsLogging.LogMessageToConsole($"[LeftGroupHook] - Group:{updatedGroup.ToStringDebug}");
      //MetricsLogging.LogMessageToInfoChat($"Group updated: {updatedGroup}");
  }
}

[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.UpdateGroupMembers))]
public class UpdateGroupMembersHook
{

  private static void Postfix(Group.Logic __instance)
  {
    var updatedGroup = MetricsGroup.RefreshGroupMembers();

    //MetricsLogging.LogMessageToConsole($"[UpdateGroupMembersHook] - Group:{updatedGroup.ToStringDebug()}");
    //MetricsLogging.LogMessageToInfoChat($"Group updated: {updatedGroup}");
  }
}

#region "unused hooks"
//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.LeaveGroup))]
//public class GroupHook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.LeaveGroup Event");

//  }
//}

//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.TryDisband))]
//public class Group3Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.TryDisband");

//  }
//}


//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.TryLeaveGroup))]
//public class Group4Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.TryLeaveGroup");

//  }
//}

//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.ReceiveInvite))]
//public class Group5Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.ReceiveInvite");

//  }
//}
//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.InviteTimedOut))]
//public class Group6Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.InviteTimedOut");

//  }
//}
//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.RequestInvite))]
//public class Group7Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.RequestInvite");

//  }
//}

//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.RequestKick))]
//public class Group8Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.RequestKick");

//  }
//}

//[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.RequestPassLeader))]
//public class Group9Hook
//{

//  private static void Postfix(Group.Logic __instance)
//  {
//    MetricsLogging.LogMessageToConsole($"Group.Logic.RequestPassLeader");

//  }
//}

#endregion

