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

namespace PantheonMetrics;

public class PantheonMetricsMain : MelonMod
{
  public const string ModVersion = "0.0.1";


  private static PropertyInfo _statusLogicEntityProp;    // EntityStatus.Logic.Entity → IEntity
  private static MethodInfo _setOverrideMethod;          // EntityStatus.Logic.SetOverride(EntityStatusType, bool)
  private static FieldInfo _statusLogicEntityField;
  private static MethodInfo _getNetworkIdMethod;
  private static Assembly _il2cppAsm;

  private static UnityEngine.Rect _debugWindowRect = new UnityEngine.Rect(10, 50, 300, 130);
  //private static UnityEngine.Rect _debugWindowRect = new UnityEngine.Rect(10, 50, 380, 130);

  private static Texture2D backgroundTexture = null;
  private static GUIStyle buttonStyleActivated = null;
  private static GUIStyle buttonStyleSubButton = null;
  private static GUIStyle boxStyle = null;

  private static bool ShowExperiencePanel { get; set; } = true;

  public override void OnInitializeMelon()
  {
    

    MetricsLogging.Log = LoggerInstance;
    MetricsExperience.ResetExperience();

    //Enable and disable Features. SHould be loaded from somewhere and should be overridable from client
    MetricsConfiguration.BreathWarningEnabled = false;
    MetricsConfiguration.ExperienceMetricEnabled = true;
    MetricsConfiguration.CombatTrackingEnabled = true;
  }

  

  public override void OnLateInitializeMelon()
  {
  }

  public override void OnUpdate()
  {
  }

  
  public override void OnGUI()
  {

    if (!MetricsConfiguration.ExperienceMetricEnabled || !MetricsPlayer.IsPlayerLoadedIntoScene)
      return;

    if (backgroundTexture == null)
      backgroundTexture = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
    if (buttonStyleActivated == null || buttonStyleSubButton == null || boxStyle == null)
      CreateStyles();

    var lastKillCount = MetricsExperience.LastKills.Count;

    float x = _debugWindowRect.x;
    float y = _debugWindowRect.y;
    float w = _debugWindowRect.width;
    float titleH = 24f;
    float lineH = 18f;

    var rectCloseOpenBtn = new UnityEngine.Rect(0, y, _debugWindowRect.x, titleH * 9);

      
    var rectHeader = new UnityEngine.Rect(x, y, w, titleH);

    var rectExpLastPeriod= new UnityEngine.Rect(x, y + (1 * titleH), w, titleH);
    var rectExpAvt = new UnityEngine.Rect(x, y + (2 * titleH), w, titleH);
    var rectExpHist = new UnityEngine.Rect(x, y + (3 * titleH), w, titleH * 5);
    var rectBtn1 = new UnityEngine.Rect(x, y + (8 * titleH), (w / 2), titleH);
    var rectBtn2 = new UnityEngine.Rect(x + (w / 2), y + (8 * titleH), (w / 2), titleH);


    if (GUI.Button(rectCloseOpenBtn, "", buttonStyleActivated)) 
    {
      ShowExperiencePanel = !ShowExperiencePanel;
    }

    if (ShowExperiencePanel)
    {
      if (GUI.Button(rectBtn1, "<color=White>Reset</color>", buttonStyleSubButton))
      {
        MetricsExperience.ResetExperience();
        MetricsLogging.LogMessageToInfoChat($"Experience logging has been reset.");

      }
      UnityEngine.GUI.Box(rectBtn2, $"", boxStyle);

      UnityEngine.GUI.Box(rectHeader, $"<b><color=Red><size={lineH - 1}>Pantheon Metrics {ModVersion}</size></color></b>", boxStyle);

      UnityEngine.GUI.Box(rectExpLastPeriod, $" <b><color=White><size={lineH - 5}> Total 10 min: {MetricsExperience.TotalExperienceTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture)}</size></color></b>", boxStyle);

      UnityEngine.GUI.Box(rectExpAvt, $" <b><color=White><size={lineH - 5}> Average/min: {MetricsExperience.ExperiencePerMinTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture)}</size></color></b>", boxStyle);

      //Experience lines
      UnityEngine.GUI.Box(rectExpHist, $"", boxStyle);

      for (int i = 0; i < lastKillCount; i++)
      {
        var entry = MetricsExperience.LastKills[lastKillCount - 1 - i];
        var rectExpEntry = new UnityEngine.Rect(x, rectExpHist.y + (i * titleH), w, titleH);
        UnityEngine.GUI.Label(rectExpEntry, $"  <color=White><size={lineH - 5}>{entry.time: HH:mm:ss}: {entry.enemy}({entry.experience.ToString("N0", CultureInfo.CurrentUICulture)})</size></color>");
      }
    }


    
  }
  private static void CreateStyles()
  {
    buttonStyleActivated = new GUIStyle();

    buttonStyleActivated.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
    buttonStyleActivated.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 1f));
    buttonStyleActivated.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1f));
    buttonStyleActivated.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 1f));
    buttonStyleActivated.alignment = TextAnchor.MiddleCenter;

    buttonStyleSubButton = new GUIStyle();

    buttonStyleSubButton.normal.background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 0.5f));
    buttonStyleSubButton.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleSubButton.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    buttonStyleSubButton.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleSubButton.alignment = TextAnchor.MiddleCenter;
    buttonStyleSubButton.border = new RectOffset();

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