using HarmonyLib;
using HarmonyLib.Tools;
using Il2Cpp;
using Il2CppTMPro;
using PantheonMetrics;
using PantheonMetrics.Data;

namespace Clock.Hooks;

/// <summary>
/// Hooks for UI panel events, specifically for creating the server time display
/// </summary>
[HarmonyPatch(typeof(UICompass), nameof(UICompass.Start))]
public class CompassHook
{
  /// <summary>
  /// Called when a UI panel starts
  /// Creates the server time display when the compass panel is initialized
  /// </summary>
  /// <param name="__instance">The UI panel that started</param>
  private static void Postfix(UICompass __instance)
  {
    // Do not block this on PlayerIsLoaded, it will exception if you do as OnUpdate() gets called before the panel is made and kaboom
    PantheonMetricsMain.CreateTimeDisplay();
  }
}