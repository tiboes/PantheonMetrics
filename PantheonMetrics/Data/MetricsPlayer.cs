using HarmonyLib;
using Il2Cpp;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using PantheonMetrics.Data;
using PantheonMetrics.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static UnityEngine.Accessibility.AccessibilityManager;


namespace PantheonMetrics.Data;

public static class MetricsPlayer
{
  
  




  public static bool IsPlayerLoadedIntoScene { get; set;  } = false;
  public static string PlayerName
  {
    get
    {
      if (PlayerGameObject ==  null && PlayerGameObject.info == null)
      {
        return "NOT SET";
      }

      return PlayerGameObject.info.DisplayName;
    }
  } 
  public static string PlayerNetworkId => PlayerGameObject?.NetworkId.Value.ToString() ?? "Unknown";


  public static Vector3 LastRecordedPosition { get; set; } = Vector3.zero;


  public static Vector3 GetPlayerPosition()
  {
    if (PlayerGameObject == null)
      return new Vector3(0, 0, 0);


    return PlayerGameObject.Position;
  }

  public static EntityPosition PlayerPosition
  {
    get
    {
      if (PlayerGameObject == null)
        return new EntityPosition(0, 0, 0, 0);
      
      
      Vector3 pos = PlayerGameObject.Position;
      //var forward = PlayerGameObject.Transform.forward;
      //float angle = (Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg + 360) % 360;

      if(LastRecordedPosition == Vector3.zero)
        LastRecordedPosition = pos;


      //var curX = pos.x;
      //var curZ = pos.z;

      //var lastX = LastRecordedPosition.x;
      //var lastZ = LastRecordedPosition.z;

      //var directionToLastRecorded = new Vector3(lastX - curX, 0, lastZ - curZ);


      //float angleToLastRecorded = (Mathf.Atan2(directionToLastRecorded.x, directionToLastRecorded.z) * Mathf.Rad2Deg + 360) % 360;


      var cardinalDirection = MetricsLocation.GetCardinalDirection(pos, LastRecordedPosition);

      MetricsLogging.LogMessageToInfoChat($"[MetricsPlayer.PlayerPosition] Last Recorded Position is to the {cardinalDirection}");


      // forward: {forwardX}::{forwardY}::{forwardZ} - norm: {norm}, mag: {mag}, 
      //MetricsLogging.LogMessageToInfoChat($"[MetricsPlayer.PlayerPosition] CUrrent: {pos} - Last Recorded: {LastRecordedPosition} - New X, Z ({lastX - curX}, {lastZ - curZ}) - Angle To Last Recorded: {((int)angleToLastRecorded)} you are facing: {angle}");




      LastRecordedPosition = pos;
      return new EntityPosition(pos.x, pos.y, pos.z, 0);
    }
  }


  private static float _maxHealth = 0;
  private static float _health =0;
  //public static bool CurrentlySubmerged { get; set; } = false;

  public static System.Collections.Generic.Dictionary<string, EntityObject> Pets { get; set; } = new System.Collections.Generic.Dictionary<string, EntityObject>();

  public static string ListPlayerPets { get
    {
      return String.Join(", ", Pets.Keys);
    } 
  }

  public static void ResetPets()
  {
    Pets = new System.Collections.Generic.Dictionary<string, EntityObject>();
  }

  public static void AddPet(EntityObject pet)
  {

    if (!Pets.ContainsKey(pet.DisplayName))
      Pets.Add(pet.DisplayName, pet);
  }
  public static void RemovePet(EntityObject pet)
  {
    Pets.Remove(pet.DisplayName);
  }



  public static EntityPlayerGameObject PlayerGameObject { get; set; }


  public static float MaxHealth => PlayerGameObject.Pools.GetPool(Il2CppPantheonPersist.PoolType.Health).Max;
  public static float OldHealth => _health;

  public static float CurrentHealth
  {
    get
    {
      if(PlayerGameObject == null)
        throw new InvalidOperationException("PlayerGameObject is not set.");

      _health = PlayerGameObject.Pools.GetPool(Il2CppPantheonPersist.PoolType.Health).Current;
      if (_maxHealth < _health)
        _maxHealth = _health;
      return _health;
    }
  }

  //public static string InCombat { 
  //  get
  //  {
  //    var status = PlayerGameObject.Status;
  //    if (status != null)
  //      return status.ToString() ?? "Unknown";
  //    return string.Empty;
  //  }
  //}



  public static float CurrentBreath
  {
    get
    {
      if (PlayerGameObject == null)
        return 100;

      return PlayerGameObject.Pools.GetPool(Il2CppPantheonPersist.PoolType.Breath).Current;
    }
  }
}
