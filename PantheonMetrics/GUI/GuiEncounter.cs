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

public class GuiEncounter : GuiTogglePanelBase
{
  public static List<string> latestEncounter;
  public static DateTime lastDataRefresh = DateTime.MinValue;
  public GuiEncounter(int x, int y, int width, int height) : base(x, y, width, height)
  {

  }

  public void EnsureInitialialization()
  {
    if (_toggleTogglePanel == null)
      _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetPanelStyle(PanelStyleEnum.Invisible), "encounter");
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

    var now = DateTime.Now;
    if (lastDataRefresh < now.AddSeconds(-1))
    {
      //TODO Mititaged damage
      List<string> result = MetricsCombat.GetEncounterResultAsLines();

      //making sure that encounter data does not disapear after combat
      if (result.Any())
        latestEncounter = result;
    }
    if (latestEncounter != null && latestEncounter.Any())
    {
      foreach (var line in latestEncounter)
        _toggleTogglePanel.AddText(new TextDefinition(line, "White", 15, false));
    }

    _toggleTogglePanel.Render();
  }
}
