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

public class GuiDPS
{
  private PanelM _dpsTogglePanel;
  private int _x;
  private int _y;
  private int _width;
  private int _height;


  public GuiDPS(int x, int y, int width, int height) 
  { 
    _x = x;
    _y = y;
    _width = width;
    _height = height;
    _dpsTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "dps");
  }

  public void EnsureInitialialization()
  {
    if (_dpsTogglePanel == null)
      _dpsTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "dps");
  }

  public void Render()
  {
    _dpsTogglePanel.ResetTextDefinitions();
    _dpsTogglePanel.SetGUIStyle(GUIGlobals.GetPanelStyle(PanelStyleEnum.Box));
    _dpsTogglePanel.AddText(new TextDefinition("Damage per second", "Red", 20, true));

    var dps = MetricsCombat.DamagePerSecond();
    

    var dpsTxt = $"{(dps == 0 ? 0 : dps):#}";

    LabelM dpsLbl = new LabelM(1, _dpsTogglePanel.X, (_dpsTogglePanel.Y + 25), _dpsTogglePanel.Width, _dpsTogglePanel.Height-20, null, new TextDefinition(dpsTxt, "Red", 75, true, UnityEngine.TextAnchor.LowerRight));



    //var totalValue = MetricsExperience.TotalExperienceTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture);
    //var avgValue = MetricsExperience.ExperiencePerMinTheLast10MinCached.ToString("N0", CultureInfo.CurrentUICulture);
    ////Line 1 
    //LabelM totalLbl = new LabelM(1, _experienceTogglePanel.X, (_experienceTogglePanel.Y + 25), _experienceTogglePanel.Width / 2, 18, null, new TextDefinition("Total 10 min", "White", 18, false, UnityEngine.TextAnchor.MiddleCenter));
    //LabelM avgLbl = new LabelM(2, totalLbl.GetLeftOf().x, totalLbl.GetLeftOf().y, totalLbl.Width, totalLbl.Height, null, new TextDefinition("Average/min", "White", 18, false, UnityEngine.TextAnchor.MiddleCenter));
    //LabelM totalValueLbl = new LabelM(3, totalLbl.GetBelow().x, totalLbl.GetBelow().y + 2, totalLbl.GetWidthHeight().width, totalLbl.GetWidthHeight().height, null, new TextDefinition(totalValue, "White", 18, false, UnityEngine.TextAnchor.MiddleCenter));
    //LabelM avgValueLbl = new LabelM(3, avgLbl.GetBelow().x, avgLbl.GetBelow().y + 2, avgLbl.GetWidthHeight().width, avgLbl.GetWidthHeight().height, null, new TextDefinition(avgValue, "White", 18, false, UnityEngine.TextAnchor.MiddleCenter));

    //var lastKillCount = MetricsExperience.LastKills.Count;
    //for (int i = 0; i < lastKillCount; i++)
    //{
    //  var entry = MetricsExperience.LastKills[lastKillCount - 1 - i];
    //  var killLine = $@"{entry.time: HH:mm:ss}: {entry.enemy}({entry.experience.ToString("N0", CultureInfo.CurrentUICulture)})";
    //  _experienceTogglePanel.AddText(new TextDefinition(killLine, "White", 16));
    //}

    _dpsTogglePanel.Render();
    dpsLbl.Render();
    //totalLbl.Render();
    //avgLbl.Render();
    //totalValueLbl.Render();
    //avgValueLbl.Render();

  }
}
