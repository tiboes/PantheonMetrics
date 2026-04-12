using Il2Cpp;
using Il2CppPantheonPersist;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsLogging
{
  public static MelonLogger.Instance Log { get; set; }

  public static void LogMessageToConsole(string message)
  {
    Log.Msg(message);
  }

  public static void LogMessageToInfoChat(string message)
  {
    UIChatWindows.Instance.PassMessage(message, ChatChannelType.Info);
  }
}
