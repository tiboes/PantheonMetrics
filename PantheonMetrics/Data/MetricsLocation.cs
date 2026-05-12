using HarmonyLib;
using Il2Cpp;
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
using static UnityEngine.Accessibility.AccessibilityManager;

namespace PantheonMetrics.Data;

public static class MetricsLocation
{
  public static Dictionary<int, (Vector3 position, string text) > SavedLocations = new Dictionary<int, (Vector3, string)>();

  private static Dictionary<float, string> CardinalDirections = new Dictionary<float, string>
  {
    { 11.25f, "North" },
    { 33.75f, "North-Northeast" },
    { 56.25f, "Northeast" },
    { 78.75f, "East-Northeast" },
    { 101.25f, "East" },
    { 123.75f, "East-Southeast" },
    { 146.25f, "Southeast" },
    { 168.75f, "South-Southeast" },
    { 191.25f, "South" },
    { 213.75f, "South-Southwest" },
    { 236.25f, "Southwest" },
    { 258.75f, "West-Southwest" },
    { 281.25f, "West" },
    { 303.75f, "West-NorthWest" },
    { 326.25f, "Northwest" },
    { 348.75f, "North-Northwest" },
    { 360.01f, "North" }
  };


  private static long ticsSinceLastDeathlocDirections = 0;
  public static EntityPosition? LastKnowDeathPosition {  get; set; }

  public static int DeathCounter { get; set; } = 0;

  public static void ResetLastKnownDeathPosistion()
  {
    LastKnowDeathPosition = null;
  }

  public static void AddDeath(EntityPosition position)
  {
    LastKnowDeathPosition = position;
    DeathCounter++;
  }

  public static float GetAngleTo(Vector3 from,  Vector3 to)
  {
    var curX = from.x;
    var curZ = from.z;

    var lastX = to.x;
    var lastZ = to.z;

    var directionToLastRecorded = new Vector3(lastX - curX, 0, lastZ - curZ);


    float angleToLastRecorded = (Mathf.Atan2(directionToLastRecorded.x, directionToLastRecorded.z) * Mathf.Rad2Deg + 360) % 360;
    return angleToLastRecorded;
  }

  public static string GetCardinalDirection(Vector3 from, Vector3 to)
  {
    if (from == to)
      return "You are there";

    var angle = GetAngleTo(from, to);
    
    var possibleDirections = CardinalDirections.Where(a => angle < a.Key);

    MetricsLogging.LogMessageToConsole($"Angle to last recorded position: {angle}, Possible directions {string.Join(", ", possibleDirections.Select(d => (d.Key, d.Value)))}");

    if (!possibleDirections.Any())
      return "No possible directions found";
    
    return possibleDirections.First().Value;
  }

  public static (string direction, float distance) GetCardinalDirectionAndDistance(Vector3 from, Vector3 to)
  {
    if (from == to)
      return ("You are there", 0);

    var angle = GetAngleTo(from, to);

    var possibleDirections = CardinalDirections.Where(a => angle < a.Key);

    //MetricsLogging.LogMessageToConsole($"Angle to last recorded position: {angle}, Possible directions {string.Join(", ", possibleDirections.Select(d => (d.Key, d.Value)))}");

    if (!possibleDirections.Any())
      return ("No possible directions found", 0);

    return (possibleDirections.First().Value, Vector3.Distance(from, to));
  }

  public static int AddLocation(int x, int y, string text)
  {
    var newKey = 1;
    if (SavedLocations.Keys.Count != 0)
      newKey = SavedLocations.Keys.Max() + 1;

    SavedLocations.Add(newKey, (new Vector3(x, 0, y), text));

    return newKey;
  }

  public static void RemoveLocation(int key)
  {
    var exists = SavedLocations.TryGetValue(key, out var v);
    if (exists)
      SavedLocations.Remove(key);

  }
  public static void ResetLocations()
  {
    SavedLocations = new Dictionary<int, (Vector3, string)>();
  }


  public static string GetDeathLocationDirections()
  {
    if (LastKnowDeathPosition == null)
      return "No known last position";


    var playerX = MetricsPlayer.PlayerGameObject.Position.x;
    var playery = MetricsPlayer.PlayerGameObject.Position.y;

    var playerWestEastDirection = playerX - LastKnowDeathPosition.X;
    var playerNorthSouthDirection =  playery - LastKnowDeathPosition.Y;

    var xDir = playerWestEastDirection < 0 ? "E" : "W";
    var yDir = playerNorthSouthDirection < 0 ? "S" : "N";


    return $"Corpse: {xDir}: {Math.Abs((int)playerWestEastDirection)}u, {yDir}: {Math.Abs((int)playerNorthSouthDirection)}u";
  }

}
