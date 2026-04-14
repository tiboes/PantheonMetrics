using HarmonyLib;
using Il2Cpp;
using Il2CppPantheonPersist;
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
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Il2CppSystem.Xml.XmlTextReaderImpl;

namespace PantheonMetrics;

public class PantheonMetricsMain : MelonMod
{
  public const string ModVersion = "1.0.0";


  private static PropertyInfo _statusLogicEntityProp;    // EntityStatus.Logic.Entity → IEntity
  private static MethodInfo _setOverrideMethod;          // EntityStatus.Logic.SetOverride(EntityStatusType, bool)
  private static FieldInfo _statusLogicEntityField;
  private static MethodInfo _getNetworkIdMethod;
  private static Assembly _il2cppAsm;

  

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
    if (!MetricsPlayer.IsPlayerLoadedIntoScene)
      return;
    
    ExperienceGUI.Render(MetricsConfiguration.ExperienceMetricEnabled);
  }


}