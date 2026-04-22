using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.GUI;
using PantheonMetrics.Logics;
using PantheonMetrics.Objects;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace PantheonMetrics.Hooks;

public static class EntityStatusLogicHook
{
  [HarmonyPatch(typeof(EntityStatus.Logic), nameof(EntityStatus.Logic.SetOverride))]
  public class LogicHook
  {
    private static void Prefix(EntityStatus.Logic __instance, EntityStatusType statusType, bool enabled)
    {
      if (__instance == null)
        return;

      if (PlayerNetworkStartHook.exitMessageReceieved)
      {
        MetricsLogging.LogMessageToConsole($"Incorrect Logging Message Received...Ignoring");
        PlayerNetworkStartHook.exitMessageReceieved = false;
        MetricsPlayer.IsPlayerLoadedIntoScene = true;
      }


      WaterManagement.HandleWaterLogic(statusType, enabled);

      List<EntityStatusType> allowedStatusTypes = new List<EntityStatusType>
      {
        EntityStatusType.InCombat,
        EntityStatusType.Dead,
        EntityStatusType.Pet,
        EntityStatusType.PetFollowing,
      };

      if (!allowedStatusTypes.Contains(statusType))
        return;

      

      //MetricsLogging.LogMessageToConsole($"EntityStatusLogicHook {statusType}-{enabled}");

      string displayName, classString, entityKind, race, nameText, subNameText, petOwnerIfAny, netId;
      bool isPlayerAccessLevel;
      long characterId;


      var entityObject = CreateEntityObject(__instance.Entity, (statusType, enabled));


      if (statusType == EntityStatusType.Pet)
      {
        //MetricsLogging.LogMessageToConsole($"[EntityStatusLogicHook.PreFix] {entityObject.DisplayName}({entityObject.NetworkId}) - {entityObject.Class} - {entityObject.Relation} -  StatusType: {statusType}({enabled})");
      }
      if (entityObject.Relation == EntityRelationEnum.Pet && entityObject.OwnerName ==  MetricsPlayer.PlayerName)
      {
        //MetricsLogging.LogMessageToConsole($"[SetOverride_Prefix] {entityObject.DisplayName}({entityObject.NetworkId}) - {entityObject.Class} -  statusType: {statusType}({enabled})");
        MetricsPlayer.AddPet(entityObject);

      }
      //TODO what messages are received when pets time expires

      //TODO 



      //its probably okay to do above before the player is loaded in. At some point it might be used to populate stuff
      if (!MetricsPlayer.IsPlayerLoadedIntoScene)
        return;

      if (statusType == EntityStatusType.Dead && enabled)
        MetricsExperience.LastRegisteredDeath = entityObject;


      if (statusType == EntityStatusType.InCombat && enabled && entityObject.IsPlayer())
      { 
        MetricsCombat.CombatStartTime = DateTime.Now;
      }

      if (MetricsCombat.CombatStartTime != null && statusType == EntityStatusType.InCombat && !enabled && entityObject.IsPlayer())
      {
        MetricsCombat.CombatEndTime = DateTime.Now;
      }
      if (statusType == EntityStatusType.Dead && enabled && entityObject.IsPlayer())
      {
        //player has died
        var x = __instance.Entity.Position.x;
        var y = __instance.Entity.Position.y;
        var z = __instance.Entity.Position.z;
        MetricsDeathKeeper.LastKnowDeathPosition = new EntityPosition(x, y, z);
        MetricsLogging.LogMessageToInfoChat($"You have died at coordinates: {x}, {y}, {z}");
      }


      //MetricsLogging.LogMessageToConsole($"[EntityStatusLogicHook.PreFix] {entityObject.DisplayName}({entityObject.NetworkId}) - {entityObject.Class} - {entityObject.Relation} -  StatusType: {statusType}({enabled})");
    }
  }

  private static EntityObject CreateEntityObject(IEntity entity, (EntityStatusType statusType, bool enabled) debugInfo)
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


    //MetricsLogging.LogMessageToConsole($"entity: entityAssembly: {entityAssembly}, entitynameSpace: {entitynameSpace}, entityname: {entityname}, entityFullName: {entityFullName},entityDescription: {entityDescription}, entityTypeInfo:{entityTypeInfo}");


    if (entity.Info != null)
    {
      displayName = entity.Info.DisplayName;
      classString = entity.Info.Class.ToString();
      characterId = entity.Info.CharacterId;
      entityKind = entity.Info.Kind.ToString();
      race = entity.Info.Race.ToString();
      entityRace = entity.Info.Race;


      if (entity.Nameplate != null)
      {
        EntityNameplate nameplate = entity.Nameplate;
        nameText = nameplate.nameText.text;
        subNameText = nameplate.subNameText.text;
        levelText = nameplate.levelText.text;
      }



      isPlayerAccessLevel = entity.Info.AccessLevel == AccessLevel.Player;
    }



    string netId = entity.NetworkId.Value.ToString();

    if (string.IsNullOrEmpty(netId))
      throw new Exception($"Could not ResolveEntityNetworkId");

    EntityObject entityObject = null;
    if (isPlayerAccessLevel)
    {


      if (netId == MetricsPlayer.PlayerNetworkId)
        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.LocalPlayer,String.Empty);
      else
        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.NonLocalPlayer, String.Empty);



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
      if (subNameText.EndsWith("'s Minion>"))//Necro, Enchanter
        owner = subNameText.Substring(1, subNameText.Length - 11);
      if (subNameText.EndsWith("'s Ward>"))//Druid
        owner = subNameText.Substring(1, subNameText.Length - 9);


      if (MetricsGroup.InGroup())
      {
        var petOwner = MetricsGroup.GetGroupMember(owner);
        if (petOwner != null)
        { 
          petOwnerIfAny = petOwner.DisplayName;
          entityRelation = EntityRelationEnum.Pet;
        }
      }
      else
      {
        if (MetricsPlayer.IsPlayerLoadedIntoScene && MetricsPlayer.PlayerName == owner)//Because this can be called before the Player is loaded into the scene, player object can be null.
        {
          petOwnerIfAny = MetricsPlayer.PlayerName;
          entityRelation = EntityRelationEnum.Pet;
        }
      }


      //TODO does not handle players outside of group

      entityObject = new EntityObject(netId, displayName, classString, entityRelation, owner);
      //TODO any situations left?
      

    }

    //MetricsLogging.LogMessageToConsole($"[SetOverride_Prefix] {displayName}({netId}) - {classString} -  statusType: {status}({__1}): IsPlayer: {isPlayerAccessLevel}, characterId: {characterId}, Kind: {entityKind}, Race: {race}, nameText; {nameText}, subNameText: {subNameText}, Pet Owner: {petOwnerIfAny}");
    return entityObject;
  }

}
