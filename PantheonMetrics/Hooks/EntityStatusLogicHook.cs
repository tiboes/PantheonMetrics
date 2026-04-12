using HarmonyLib;
using Il2Cpp;
using Il2CppNpcStates;
using Il2CppPantheonPersist;
using Il2CppSystem.Runtime.Serialization;
using MelonLoader;
using PantheonMetrics.Data;
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


      //var iEntityType = _il2cppAsm.GetType("Il2Cpp.IEntity")
      //           ?? _il2cppAsm.GetTypes().FirstOrDefault(t => t.Name == "IEntity" && t.IsInterface);
      //_getNetworkIdMethod = iEntityType.GetMethod("get_NetworkId");

      //Check if this is regarding the player
      //Place player handling in seperate method
      //Check if this is 




      string displayName, classString, entityKind, race, nameText, subNameText, petOwnerIfAny, netId;
      bool isPlayerAccessLevel;
      long characterId;


      var entityObject = CreateEntityObject(__instance.Entity);

      //its probably okay to do above before the player is loaded in. At some point it might be used to populate stuff
      if (!MetricsPlayer.IsPlayerLoadedIntoScene)
        return;

      if (statusType == EntityStatusType.Dead && enabled)
        MetricsExperience.LastRegisteredDeath = entityObject;

      MetricsLogging.LogMessageToConsole($"[EntityStatusLogicHook.PreFix] {entityObject.DisplayName}({entityObject.NetworkId}) - {entityObject.Class} - {entityObject.Relation} -  StatusType: {statusType}({enabled})");
    }
  }

  private static EntityObject CreateEntityObject(IEntity entity)
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

      EntityNameplate nameplate = entity.Nameplate;
      nameText = nameplate.nameText.text;
      subNameText = nameplate.subNameText.text;
      levelText = nameplate.levelText.text;

      isPlayerAccessLevel = entity.Info.AccessLevel == AccessLevel.Player;
    }





    string netId = entity.NetworkId.Value.ToString();

    if (string.IsNullOrEmpty(netId))
      throw new Exception($"Could not ResolveEntityNetworkId");

    EntityObject entityObject = null;
    if (isPlayerAccessLevel)
    {
      if (netId == MetricsPlayer.PlayerNetworkId)
        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.LocalPlayer);
      else
        entityObject = new EntityObject(netId, displayName, classString, EntityRelationEnum.NonLocalPlayer);
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

}
