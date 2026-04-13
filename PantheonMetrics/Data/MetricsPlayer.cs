using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PantheonMetrics.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Accessibility.AccessibilityManager;

namespace PantheonMetrics.Data;

public static class MetricsPlayer
{
  public static bool IsPlayerLoadedIntoScene { get; set;  } = false;
  public static string PlayerName
  {
    get
    {
      if (PlayerGameObject ==  null)
        MetricsLogging.LogMessageToConsole($"[MetricsPlayer.PlayerName] is null");
      else if (PlayerGameObject.info == null)
        MetricsLogging.LogMessageToConsole($"[MetricsPlayer.PlayerNam.Info] is null");


      return PlayerGameObject.info.DisplayName;
    }
  } 
  public static string PlayerNetworkId => PlayerGameObject?.NetworkId.Value.ToString() ?? "Unknown";


  private static float _maxHealth = 0;
  private static float _health =0;
  //public static bool CurrentlySubmerged { get; set; } = false;






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
