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

public class GuiInfo: GuiTogglePanelBase
{
  public GuiInfo(int x, int y, int width, int height) : base(x, y, width, height)
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

    _toggleTogglePanel.AddText(new TextDefinition("Slash commands", "Red", 25, true));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics - Open metrics window.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics loc x y - Add tracking loc.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics loc x y ABC - Add tracking loc with text.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics loc remove id - Remove specific tracking.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics loc reset - Remove all trackings.", "White", 15, false));

    _toggleTogglePanel.AddText(new TextDefinition("/metrics enc mitigated - Toggle mitigated.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics enc attacker - Toggle attacker breakdown.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics enc ability - Toggle ability breakdown.", "White", 15, false));
    _toggleTogglePanel.AddText(new TextDefinition("/metrics enc type - Toggle damage type breakdown.", "White", 15, false));

    _toggleTogglePanel.Render();
  }
}
