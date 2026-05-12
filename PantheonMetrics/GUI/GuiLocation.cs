using PantheonMetrics.Data;
using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI;

public class GuiLocation : GuiTogglePanelBase
{
  public GuiLocation(int x, int y, int width, int height) : base(x, y, width, height)
  {

  }

  public void EnsureInitialialization()
  {
    if (_toggleTogglePanel == null)
      _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetPanelStyle(PanelStyleEnum.Invisible), "info");
  }


  public void Render(int x, int y)
  {
    _x = x; 
    _y = y;
    _toggleTogglePanel.UpdateXY(_x, _y);
    Render();
  }

  public override void Render()
  {
    _toggleTogglePanel.ResetTextDefinitions();
    _toggleTogglePanel.SetGUIStyle(GUIGlobals.GetPanelStyle(PanelStyleEnum.Invisible));
    var playerPos = MetricsPlayer.GetPlayerPosition();
    var pPos = new Vector3(playerPos.x, 0, playerPos.z);
    foreach (var loc in MetricsLocation.SavedLocations)
    {
      var id = loc.Key;
      var location = loc.Value;
      var directionAndDistance = MetricsLocation.GetCardinalDirectionAndDistance(pPos, location.position);
      _toggleTogglePanel.AddText(new TextDefinition($"({id}) - {location.text} - {directionAndDistance.direction}({(int)directionAndDistance.distance})", "White", 15));
    }

    _toggleTogglePanel.Render();
  }
}
