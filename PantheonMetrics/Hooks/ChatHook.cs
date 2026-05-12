using HarmonyLib;
using Il2Cpp;
using Il2CppPantheonPersist;
using PantheonMetrics;
using PantheonMetrics.Data;

namespace PantheonMetrics.Hooks;


//[HarmonyPatch(typeof(EntityClientMessaging.Logic), nameof(EntityClientMessaging.Logic.ReceiveChatMessage), typeof(string), typeof(string), typeof(ChatChannelType), typeof(IEntity))]
//public class ReceiveChatMessageHook
//{
//  private static bool Prefix(EntityClientMessaging.Logic __instance, string name, string message, ChatChannelType channel, IEntity fromEntity = null)
//  {
//    MetricsLogging.LogMessageToConsole($"name: {name} message: {message}, channel: {channel}");
//    if(fromEntity != null)
//    {
//      MetricsLogging.LogMessageToConsole($"message from entity: {fromEntity.GetType()}");
//    }
//    if (__instance != null)
//    {
//      MetricsLogging.LogMessageToConsole($"Instance: {__instance.GetType()}");
//    }

//    return true;
//  }
//}


[HarmonyPatch(typeof(EntityClientMessaging.Logic), nameof(EntityClientMessaging.Logic.SendChatMessage), typeof(string), typeof(ChatChannelType))]
public class SendChatMessageHook
{
  private static bool Prefix(EntityClientMessaging.Logic __instance, string message, ChatChannelType channel)
  {
    return MetricsConfiguration.HandleSlashCommand(message);
    //if (MetricsPlayer.IsPlayerLoadedIntoScene)
    //{
    //  if (message == "/metrics")
    //  {
    //    // The clock is already loaded and running via another hook, all we have to do is show it
    //    PantheonMetricsMain.ShowMetrics();
    //    return false;
    //  }
    //}
    //return true;
  }
}

[HarmonyPatch(typeof(EntityClientMessaging.Logic), nameof(EntityClientMessaging.Logic.RequestWhisper))]
public class RequestWhisperHook
{
  private static bool Prefix(UIChatInput __instance, string targetPlayerName, string message)
  {
    return MetricsConfiguration.HandleSlashCommand(message);


    //if (MetricsPlayer.IsPlayerLoadedIntoScene)
    //{

    //  if (message == "/metrics")
    //  {
    //    // The clock is already loaded and running via another hook, all we have to do is show it
    //    PantheonMetricsMain.ShowMetrics();
    //    return false;
    //  }
    //}
    //return true;
  }
}