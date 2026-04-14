using HarmonyLib;
using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppTMPro;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.Hooks;
using PantheonMetrics.Logics;
using PantheonMetrics.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Il2CppSystem.Xml.XmlTextReaderImpl;

namespace PantheonMetrics.GUI;

public static class ExperienceGUI
{
  private static UnityEngine.Rect _debugWindowRect = new UnityEngine.Rect(15, 50, 300, 130);
  //private static UnityEngine.Rect _debugWindowRect = new UnityEngine.Rect(10, 50, 380, 130);

  private static Texture2D backgroundTexture = null;
  private static GUIStyle buttonStyleActivated = null;
  private static GUIStyle buttonStyleStopped = null;
  private static GUIStyle buttonStyleStartted = null;
  private static GUIStyle buttonStyleResetExperience = null;
  private static GUIStyle boxStyle = null;

  private static bool ShowExperiencePanel { get; set; } = true;

  public static void Render(bool enabled)
  {
    


    float x = _debugWindowRect.x;
    float y = _debugWindowRect.y;
    float w = _debugWindowRect.width;
    float titleH = 24f;
    float lineH = 18f;

    if (!enabled)
    {
      var rectPlayBtn = new UnityEngine.Rect(0, y, _debugWindowRect.x, _debugWindowRect.x);
      if (UnityEngine.GUI.Button(rectPlayBtn, ">", buttonStyleStopped))
      {
        MetricsConfiguration.ExperienceMetricEnabled = true;
        MetricsLogging.LogMessageToInfoChat($"Experience logging has resumed.");
      }
      return;
    }

    var lastKillCount = MetricsExperience.LastKills.Count;

    var rectPauseBtn = new UnityEngine.Rect(0, y, _debugWindowRect.x, _debugWindowRect.x);
    var rectPauseResetBtn = new UnityEngine.Rect(0, (y + _debugWindowRect.x), _debugWindowRect.x, _debugWindowRect.x);
    var rectCloseOpenBtn = new UnityEngine.Rect(0, (y + 2 * _debugWindowRect.x), _debugWindowRect.x, (titleH * 9 - 2 * _debugWindowRect.x));


    var rectHeader = new UnityEngine.Rect(x, y + (1 * titleH), w, titleH);

    var rectExpLastPeriod = new UnityEngine.Rect(x, y + (2 * titleH), w, titleH);
    var rectExpPerMin = new UnityEngine.Rect(x, y + (3 * titleH), w, titleH);
    var rectExpHististory = new UnityEngine.Rect(x, y + (4 * titleH), w, titleH * 5);

    if (UnityEngine.GUI.Button(rectPauseBtn, "||", buttonStyleStartted))
    {
      MetricsConfiguration.ExperienceMetricEnabled = false;
      MetricsLogging.LogMessageToInfoChat($"Experience logging has been suspended.");
      return;
    }

    if (UnityEngine.GUI.Button(rectPauseResetBtn, "R", buttonStyleResetExperience))
    {
      MetricsExperience.ResetExperience();
      MetricsLogging.LogMessageToInfoChat($"Experience logging has been reset.");
    }

    var collapsableTxt = GUIUtils.GetGUIText((ShowExperiencePanel ? "<" : ">"), (ShowExperiencePanel ? "Grey" : "Grey"), (int)(lineH - 1), true);
    if (UnityEngine.GUI.Button(rectCloseOpenBtn, collapsableTxt, buttonStyleActivated))
    {
      ShowExperiencePanel = !ShowExperiencePanel;
    }

    if (ShowExperiencePanel)
    {
      UnityEngine.GUI.Box(rectHeader, GUIUtils.GetGUIText($"Pantheon Metrics {PantheonMetricsMain.ModVersion}", "Red", (int)(lineH - 1), true), boxStyle);
      UnityEngine.GUI.Box(rectExpLastPeriod, GUIUtils.GetGUIText($" Total 10 min: {MetricsExperience.TotalExperienceTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture)}", "White", (int)(lineH - 5), true), boxStyle);
      UnityEngine.GUI.Box(rectExpPerMin, GUIUtils.GetGUIText($@" Average/min: {MetricsExperience.ExperiencePerMinTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture)}", "White", (int)(lineH - 5), true), boxStyle);

      //Experience lines
      UnityEngine.GUI.Box(rectExpHististory, $"", boxStyle);

      for (int i = 0; i < lastKillCount; i++)
      {
        var entry = MetricsExperience.LastKills[lastKillCount - 1 - i];
        var rectExpEntry = new UnityEngine.Rect(x, rectExpHististory.y + (i * titleH), w, titleH);
        UnityEngine.GUI.Label(rectExpEntry, GUIUtils.GetGUIText($@"{entry.time: HH:mm:ss}: {entry.enemy}({entry.experience.ToString("N0", CultureInfo.CurrentUICulture)})", "White", (int)(lineH - 5)));
      }

    }
  }

  public static void InitializeRenderObjects()
  {
    if (backgroundTexture != null)
      backgroundTexture.MarkDirty();

    backgroundTexture = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
    MetricsLogging.LogMessageToInfoChat($"Loading Textures.");
    CreateStyles();
    MetricsLogging.LogMessageToInfoChat($"Loading Styles.");
  }



  private static void CreateStyles()
  {
    buttonStyleActivated = new GUIStyle();

    buttonStyleActivated.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
    buttonStyleActivated.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 1f));
    buttonStyleActivated.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1f));
    buttonStyleActivated.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 1f));
    buttonStyleActivated.alignment = TextAnchor.MiddleLeft;

    buttonStyleResetExperience = new GUIStyle();

    buttonStyleResetExperience.normal.background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 0.5f));
    buttonStyleResetExperience.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleResetExperience.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    buttonStyleResetExperience.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleResetExperience.alignment = TextAnchor.MiddleCenter;
    buttonStyleResetExperience.border = new RectOffset();

    buttonStyleStopped = new GUIStyle();

    buttonStyleStopped.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    //buttonStyleStarted.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStopped.active.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleStopped.hover.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    buttonStyleStopped.alignment = TextAnchor.MiddleCenter;
    buttonStyleStopped.border = new RectOffset();

    buttonStyleStartted = new GUIStyle();

    buttonStyleStartted.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    //buttonStyleStopped.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStartted.active.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleStartted.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStartted.alignment = TextAnchor.MiddleCenter;
    buttonStyleStartted.border = new RectOffset();




    boxStyle = new GUIStyle();
    boxStyle.normal.background = backgroundTexture; //MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
    boxStyle.margin.left = 2;
  }
  private static Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
    {
      pix[i] = col;
    }
    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();
    return result;
  }


}
