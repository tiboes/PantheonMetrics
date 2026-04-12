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

  public override void OnInitializeMelon()
  {
    MetricsLogging.Log = LoggerInstance;
    MetricsExperience.ResetExperience(0);



    //Enable and disable Features. SHould be loaded from somewhere and should be overridable from client
    MetricsConfiguration.BreathWarningEnabled = false;
    MetricsConfiguration.ExperienceMetricEnabled = true;
    MetricsConfiguration.CombatTrackingEnabled = true;
  }

  public override void OnLateInitializeMelon()
  {
    Initialize();
  }

  

  public override void OnUpdate()
  {
  }

  public override void OnGUI()
  {

  }

 


  


  private static void SetOverride_Prefix(object __instance, object __0, bool __1)
  {
    EntityStatusType status = EntityStatusType.None;
    if (__0 is EntityStatusType est)
      status = est;



    WaterManagement.HandleWaterLogic(status, __1);

    List<EntityStatusType> allowedStatusTypes = new List<EntityStatusType>
    {
      EntityStatusType.InCombat,
      EntityStatusType.Dead,
      EntityStatusType.Pet,
      EntityStatusType.PetFollowing,
    };

    if (!allowedStatusTypes.Contains(status))
      return;



    //List<EntityStatusType> ignoredStatusTypes = new List<EntityStatusType> 
    //{ 
    //  EntityStatusType.None,
    //  EntityStatusType.Swimming,
    //  EntityStatusType.Submerged,
    //  EntityStatusType.HoldingSomethingBurning,
    //  EntityStatusType.Walking,
    //};


    //if (ignoredStatusTypes.Contains(status)) 
    //  return;








    //MetricsLogging.LogMessageToConsole($"[SetOverride_Prefix] {MetricsPlayer.InCombat}");

    // Only care about: InCombat going FALSE (leash/death), or Dead going TRUE (death)
    //if (statusType == EntityStatusType_InCombat && __1) return;      // InCombat=true: ignore (PCM handles start)
    //if (statusType == EntityStatusType_Dead && !__1) return;          // Dead=false: respawn, ignore
    //if (statusType != EntityStatusType_InCombat && statusType != EntityStatusType_Dead) return;












    var iEntityType = _il2cppAsm.GetType("Il2Cpp.IEntity")
               ?? _il2cppAsm.GetTypes().FirstOrDefault(t => t.Name == "IEntity" && t.IsInterface);
    _getNetworkIdMethod = iEntityType.GetMethod("get_NetworkId");

    //Check if this is regarding the player
    //Place player handling in seperate method
    //Check if this is 




    string displayName, classString, entityKind, race, nameText, subNameText, petOwnerIfAny, netId;
    bool isPlayerAccessLevel;
    long characterId;

    var entityObject = GetEntityFromInstance(__instance, __0);

    //its probably okay to do above before the player is loaded in. At some point it might be used to populate stuff
    if (!MetricsPlayer.IsPlayerLoadedIntoScene)
      return;

    if (status == EntityStatusType.Dead && __1)
      MetricsExperience.LastRegisteredDeath = entityObject;

    MetricsLogging.LogMessageToConsole($"[SetOverride_Prefix] {entityObject.DisplayName}({entityObject.NetworkId}) - {entityObject.Class} - {entityObject.Relation} -  StatusType: {status}({__1})");

    
  }

  private static EntityObject GetEntityFromInstance(object __instance, object __0)
  {
    string displayName = String.Empty;
    string classString = String.Empty;
    bool isPlayerAccessLevel = false;
    long characterId = 0;
    string entityKind = String.Empty;
    string race = String.Empty;
    string nameText = String.Empty;
    string subNameText = String.Empty;
    string levelText = String.Empty;
    string petOwnerIfAny = "Not a pet";
    EntityRace entityRace = EntityRace.Horse;

    object entity = ResolveEntityFromLogic(__instance);
    if (entity == null)
      throw new Exception($"Could not ResolveEntityFromLogic");

    if (entity is IEntity ient)
    {
      if (ient.Info != null)
      {
        displayName = ient.Info.DisplayName;
        classString = ient.Info.Class.ToString();
        characterId = ient.Info.CharacterId;
        entityKind = ient.Info.Kind.ToString();
        race = ient.Info.Race.ToString();
        entityRace = ient.Info.Race;

        EntityNameplate nameplate = ient.Nameplate;
        nameText = nameplate.nameText.text;
        subNameText = nameplate.subNameText.text;
        levelText = nameplate.levelText.text;

        isPlayerAccessLevel = ient.Info.AccessLevel == AccessLevel.Player;
      }
    }


    string netId = ResolveEntityNetworkId(entity);
    //NetworkId(


    if (string.IsNullOrEmpty(netId))
      throw new Exception($"Could not ResolveEntityNetworkId");

    


    var entityTypeString = entity.GetType().Namespace + "::" + entity.GetType().Name;
    var zeroTypeName = __0.GetType().Namespace + "::" + __0.GetType().Name;

    EntityObject entityObject = null;
    if (isPlayerAccessLevel)
    {
      if (netId == MetricsPlayer.PlayerNetworkId)
        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.LocalPlayer);
      else
      {
        MetricsLogging.LogMessageToConsole($"NonLocalPlayer: {netId}-{MetricsPlayer.PlayerNetworkId} = {netId == MetricsPlayer.PlayerNetworkId}");

        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.NonLocalPlayer); 
      }
    }
    else
    {
      var entityRelation = EntityRelationEnum.Monster;
      switch (entityRace)
      {
        //case EntityRace.FuryArcamental: entityRelation = EntityRelationEnum.Pet; break;
        //case EntityRace.TitanArcamental: entityRelation = EntityRelationEnum.Pet; break;
        //case EntityRace.UndineArcamental: entityRelation = EntityRelationEnum.Pet; break;
        //case EntityRace.ZephyrArcamental: entityRelation = EntityRelationEnum.Pet; break;
        case EntityRace.Horse: entityRelation = EntityRelationEnum.Pet; break;
          //case EntityRace.Wolf: entityRelation = EntityRelationEnum.Pet; break;
      }


      string owner = String.Empty;
      if (subNameText.EndsWith("Companion>"))//Shaman, Summoner
        owner = subNameText.Substring(1, subNameText.Length - 14);
      if (subNameText.EndsWith("'s Minion>"))//Necro
        owner = subNameText.Substring(1, subNameText.Length - 11);
      if (subNameText.EndsWith("'s Ward>"))//Druid
        owner = subNameText.Substring(1, subNameText.Length - 9);


      if (MetricsGroup.InGroup())
      {
        var petOwner = MetricsGroup.GetGroupMember(owner);
        if (petOwner != null)
          petOwnerIfAny = petOwner.DisplayName;
      }
      else
      {
        if (MetricsPlayer.IsPlayerLoadedIntoScene && MetricsPlayer.PlayerName == owner)//Because this can be called before the Player is loaded into the scene, player object can be null.
          petOwnerIfAny = MetricsPlayer.PlayerName;
      }
      //TODO does not handle players outside of group

      entityObject = new EntityObject(netId, displayName, classString, entityRelation);
      //TODO any situations left?

      
    }
    //MetricsLogging.LogMessageToConsole($"[SetOverride_Prefix] {displayName}({netId}) - {classString} -  statusType: {status}({__1}): IsPlayer: {isPlayerAccessLevel}, characterId: {characterId}, Kind: {entityKind}, Race: {race}, nameText; {nameText}, subNameText: {subNameText}, Pet Owner: {petOwnerIfAny}");
    return entityObject;
  }


  private static void Initialize()
  {
    MetricsLogging.LogMessageToConsole($"OverrideHook - Initialize");

    _il2cppAsm = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "Il2CppScripts");


    // ── IEntity interface: Info, Nameplate, NetworkId ───
    var iEntityType = _il2cppAsm.GetType("Il2Cpp.IEntity")
        ?? _il2cppAsm.GetTypes().FirstOrDefault(t => t.Name == "IEntity" && t.IsInterface);


    var _getInfoMethod = iEntityType.GetMethod("get_Info");
    var _getNameplateMethod = iEntityType.GetMethod("get_Nameplate");
    var _getNetworkIdMethod = iEntityType.GetMethod("get_NetworkId");



    var _getStatusMethod = iEntityType.GetMethod("get_Status");
    if (_getStatusMethod != null)
    {

      var statusLogicType = _getStatusMethod.ReturnType;
      var _hasStatusMethod = statusLogicType.GetMethod("HasStatus",
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

      if (statusLogicType != null)
      {
        var over = statusLogicType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (over != null)
        {
          // Cache SetOverride for event-based encounter detection
          var v = over.FirstOrDefault(m => m.Name == "SetOverride" && m.GetParameters().Length == 2);
          _setOverrideMethod = v;
        }

        // Cache Entity property on Logic for resolving entity from SetOverride callback
#pragma warning disable CS8601 // Possible null reference assignment.
        _statusLogicEntityProp = statusLogicType.GetProperty("Entity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (_statusLogicEntityProp == null)
          _statusLogicEntityField = statusLogicType.GetField("Entity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
#pragma warning restore CS8601 // Possible null reference assignment.

      }

    }

    var prefix = typeof(PantheonMetricsMain).GetMethod(nameof(SetOverride_Prefix),
                    BindingFlags.Static | BindingFlags.NonPublic);


    var _harmony = new HarmonyLib.Harmony("com.teiabo.pantheon.metrics");
    _harmony.Patch(_setOverrideMethod, prefix: new HarmonyMethod(prefix));
  }
  private static string ResolveEntityNetworkId(object entity)
  {
    if (entity == null || _getNetworkIdMethod == null) return null;
    try
    {
      var netId = _getNetworkIdMethod.Invoke(entity, null);

      if (netId is Il2CppViNL.NetworkId n)
      {
        return n.Value.ToString();
      } 

      return netId?.ToString();
    }
    catch { return null; }
  }

  private static object ResolveEntityFromLogic(object logic)
  {
    if (logic == null) return null;
    try
    {
      if (_statusLogicEntityProp != null)
        return _statusLogicEntityProp.GetValue(logic);
      if (_statusLogicEntityField != null)
        return _statusLogicEntityField.GetValue(logic);

      // Scan for entity field
      foreach (var f in logic.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
      {
        if (f.Name.ToLower().Contains("entity"))
        {
          var val = f.GetValue(logic);
          if (val != null) return val;
        }
      }
    }
    catch { }
    return null;
  }


}