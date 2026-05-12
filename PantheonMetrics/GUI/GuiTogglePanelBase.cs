using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.GUI;

public abstract class GuiTogglePanelBase
{
  protected PanelM _toggleTogglePanel;
  protected int _x;
  protected int _y;
  protected int _width;
  protected int _height;


  public GuiTogglePanelBase(int x, int y, int width, int height)
  {
    _x = x;
    _y = y;
    _width = width;
    _height = height;
    _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "");
  }

  public abstract void Render();
}
