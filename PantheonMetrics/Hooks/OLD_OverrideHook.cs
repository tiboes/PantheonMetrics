//using PantheonMetrics.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HarmonyLib;
//using Il2Cpp;
//using Il2CppPantheonPersist;
//using MelonLoader;
//using PantheonMetrics.Data;
//using System.Reflection;
//using static Il2CppSystem.Xml.XmlTextReaderImpl;

//namespace PantheonMetrics.Hooks;

//public static class OverrideHook
//{
//  private static PropertyInfo _statusLogicEntityProp;    // EntityStatus.Logic.Entity → IEntity
//  private static MethodInfo _setOverrideMethod;          // EntityStatus.Logic.SetOverride(EntityStatusType, bool)
//  private static FieldInfo _statusLogicEntityField;
//  private static MethodInfo _getNetworkIdMethod;
//  private static Assembly _il2cppAsm;


//  public static void Initialize()
//  {
//    MetricsLogging.LogMessageToConsole($"OverrideHook - Initialize");

//    _il2cppAsm = AppDomain.CurrentDomain.GetAssemblies()
//            .FirstOrDefault(a => a.GetName().Name == "Il2CppScripts");

//    MetricsLogging.LogMessageToConsole($"OverrideHook - 1");

//    // ── IEntity interface: Info, Nameplate, NetworkId ───
//    var iEntityType = _il2cppAsm.GetType("Il2Cpp.IEntity")
//        ?? _il2cppAsm.GetTypes().FirstOrDefault(t => t.Name == "IEntity" && t.IsInterface);

//    MetricsLogging.LogMessageToConsole($"OverrideHook - 2");

//    var _getInfoMethod = iEntityType.GetMethod("get_Info");
//    var _getNameplateMethod = iEntityType.GetMethod("get_Nameplate");
//    var _getNetworkIdMethod = iEntityType.GetMethod("get_NetworkId");

//    MetricsLogging.LogMessageToConsole($"OverrideHook - 3");


//    var _getStatusMethod = iEntityType.GetMethod("get_Status");
//    if (_getStatusMethod != null)
//    {
//      MetricsLogging.LogMessageToConsole($"OverrideHook - 4");

//      var statusLogicType = _getStatusMethod.ReturnType;
//      var _hasStatusMethod = statusLogicType.GetMethod("HasStatus",
//          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

//      if (statusLogicType != null)
//      {
//        MetricsLogging.LogMessageToConsole($"OverrideHook - 5");
//        var over = statusLogicType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//        if (over != null) 
//        {
//          // Cache SetOverride for event-based encounter detection
//          var v = over.FirstOrDefault(m => m.Name == "SetOverride" && m.GetParameters().Length == 2);
//          _setOverrideMethod = v;
//        }

//        // Cache Entity property on Logic for resolving entity from SetOverride callback
//#pragma warning disable CS8601 // Possible null reference assignment.
//        _statusLogicEntityProp = statusLogicType.GetProperty("Entity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//        if (_statusLogicEntityProp == null)
//          _statusLogicEntityField = statusLogicType.GetField("Entity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//#pragma warning restore CS8601 // Possible null reference assignment.

//        MetricsLogging.LogMessageToConsole($"OverrideHook - 6");
//      }

//    }
//    MetricsLogging.LogMessageToConsole($"OverrideHook - 7");

//    var prefix = typeof(ModMain).GetMethod(nameof(SetOverride_Prefix),
//                    BindingFlags.Static | BindingFlags.NonPublic);

//    MetricsLogging.LogMessageToConsole($"OverrideHook - 8");

//    var _harmony = new HarmonyLib.Harmony("com.awiz.pantheon.combatlog");
//    _harmony.Patch(_setOverrideMethod, prefix: new HarmonyMethod(prefix));
//    MetricsLogging.LogMessageToConsole($"OverrideHook - 9");
//  }


//  private static void SetOverride_Prefix(object __instance, object __0, bool __1)
//  {
//    MetricsLogging.LogMessageToConsole($"SetOverride_Prefix");
//    int statusType = Convert.ToInt32(__0);

//    // Only care about: InCombat going FALSE (leash/death), or Dead going TRUE (death)
//    //if (statusType == EntityStatusType_InCombat && __1) return;      // InCombat=true: ignore (PCM handles start)
//    //if (statusType == EntityStatusType_Dead && !__1) return;          // Dead=false: respawn, ignore
//    //if (statusType != EntityStatusType_InCombat && statusType != EntityStatusType_Dead) return;




//    var iEntityType = _il2cppAsm.GetType("Il2Cpp.IEntity")
//                   ?? _il2cppAsm.GetTypes().FirstOrDefault(t => t.Name == "IEntity" && t.IsInterface);


//    _getNetworkIdMethod = iEntityType.GetMethod("get_NetworkId");


//    object entity = ResolveEntityFromLogic(__instance);
//    if (entity == null) return;

//    string netId = ResolveEntityNetworkId(entity);
//    if (string.IsNullOrEmpty(netId)) return;


//    MetricsLogging.LogMessageToConsole($"NetId: {netId} - instance: {__instance.GetType().Name}, 0: {__0.GetType().Name}, 1: {__1}, statusType: {statusType}");
//  }




//  private static string ResolveEntityNetworkId(object entity)
//  {
//    if (entity == null || _getNetworkIdMethod == null) return null;
//    try
//    {
//      var netId = _getNetworkIdMethod.Invoke(entity, null);
//      return netId?.ToString();
//    }
//    catch { return null; }
//  }

//  private static object ResolveEntityFromLogic(object logic)
//  {
//    if (logic == null) return null;
//    try
//    {
//      if (_statusLogicEntityProp != null)
//        return _statusLogicEntityProp.GetValue(logic);
//      if (_statusLogicEntityField != null)
//        return _statusLogicEntityField.GetValue(logic);

//      // Scan for entity field
//      foreach (var f in logic.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
//      {
//        if (f.Name.ToLower().Contains("entity"))
//        {
//          var val = f.GetValue(logic);
//          if (val != null) return val;
//        }
//      }
//    }
//    catch { }
//    return null;
//  }

//}
