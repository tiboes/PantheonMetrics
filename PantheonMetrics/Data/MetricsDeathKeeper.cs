using PantheonMetrics.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.Data;

public static class MetricsDeathKeeper
{
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
