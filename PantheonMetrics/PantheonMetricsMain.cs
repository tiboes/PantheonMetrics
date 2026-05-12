using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Generator.MetadataAccess;
using Il2CppInterop.Runtime;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using Il2CppTMPro;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.GUI;
using PantheonMetrics.Hooks;
using PantheonMetrics.Logics;
using PantheonMetrics.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Scenes;
using UnityEngine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using TMPro;
using UnityEngine.UI;
using static Il2CppSystem.Xml.XmlTextReaderImpl;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;


namespace PantheonMetrics;


public class PantheonMetricsMain : MelonMod
{
  public const string ModVersion = "1.0.1";
  private DateTime _lastAliveCheck = DateTime.MinValue;
  private static ModWindow modWindow = new ModWindow();


  private static PropertyInfo _statusLogicEntityProp;    // EntityStatus.Logic.Entity → IEntity
  private static MethodInfo _setOverrideMethod;          // EntityStatus.Logic.SetOverride(EntityStatusType, bool)
  private static FieldInfo _statusLogicEntityField;
  private static MethodInfo _getNetworkIdMethod;
  private static Assembly _il2cppAsm;
  private static float width = 500;
  private static float height = 500;



  public Canvas parentCanvas;
  public TMP_InputField inputFieldPrefab;

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
    //
    var now = DateTime.Now;
    if (_lastAliveCheck < now.AddSeconds(-60))
    {
      _lastAliveCheck = now;
      //MetricsLogging.LogMessageToConsole($"Mod Alive Check. Player in scene: {MetricsPlayer.IsPlayerLoadedIntoScene}, GameplayerObject is null - {MetricsPlayer.PlayerGameObject == null} - { MetricsPlayer.PlayerGameObject?.NetworkId.ToString()}");
    }

    if (MetricsCombat.CombatEndTime != null && MetricsCombat.CombatStartTime != null) 
    {
      if (MetricsCombat.CombatEndTime.Value.AddMilliseconds(100) > now)
      {

        //MetricsLogging.LogMessageToInfoChat($"Combat has ended. Duration {MetricsCombat.GetEncounterDuration()} - DPS: {MetricsCombat.GetEncounterDps()}");
        //MetricsLogging.LogMessageToInfoChat(MetricsCombat.GetEncounterResult().Outstring);

        //Forcing encounter data even if not on encounter tab
        MetricsCombat.GetEncounterResultAsLines();
        MetricsCombat.ResetEncounter();
      }

    }


  }

  public static void ShowMetrics()
  {
    if (MetricsPlayer.IsPlayerLoadedIntoScene)
    {
      modWindow.ShowWindow();
    }
  }

  public static void CreateTimeDisplay()
  {
    // Build the panel and render it
    modWindow.DisplayPanel("ClockPanel", UIPanelRoots.Instance.Mid.transform, new Vector2(width, height));
  }

  public override void OnGUI()
  {
    if (!MetricsPlayer.IsPlayerLoadedIntoScene)
      return;

    //GuiLeftBar.Render();

    ModWindowButtons.Render();
  }

}