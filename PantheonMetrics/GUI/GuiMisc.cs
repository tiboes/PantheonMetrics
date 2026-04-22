using PantheonMetrics.Data;
using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.GUI;

public class GuiMisc : GuiTogglePanelBase
{
  public GuiMisc(int x, int y, int width, int height) : base(x, y, width, height)
  {

  }

  public int ResetDeathLocation(int id, string str)
  {
    MetricsDeathKeeper.ResetLastKnownDeathPosistion();
    return 0;
  }

  public int PrintCorpseDirection(int id, string str)
  {
    var directions = MetricsDeathKeeper.GetDeathLocationDirections();
    MetricsLogging.LogMessageToInfoChat(directions);
    return 0;
  }

  public void EnsureInitialialization()
  {
    if (_toggleTogglePanel == null)
      _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "Experience");
  }


  public override void Render()
  {
    int yOffset = 0;
    if (MetricsDeathKeeper.LastKnowDeathPosition != null)
    {
      //
      LabelM lastDeathLbl = new LabelM(0, _x, _y, 50, 20, null, new TextDefinition($"Death Loc", "White", 15) );
      ButtonM resetDeathLocBtn = new ButtonM(0, lastDeathLbl.GetLeftOf().x + 10, _y, 40, 20, ResetDeathLocation, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "Reset", "ResetDeathLoc");
      LabelM deathTextLbl = new LabelM(0, _x, lastDeathLbl.GetBelow().y + 2, _width, 20, null, new TextDefinition($"{MetricsDeathKeeper.LastKnowDeathPosition}", "White", 15));
      ButtonM directionsBtn = new ButtonM(0, _x , deathTextLbl.GetBelow().x +2, 60, 20, PrintCorpseDirection, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "Get Directions to Corpse", "CorpseDir");


      //Some way to track the direction E.G. NW of current location


      lastDeathLbl.Render();
      resetDeathLocBtn.Render();
      deathTextLbl.Render();
      directionsBtn.Render();
      yOffset += 62;
    }
    //Show 
  }
}
