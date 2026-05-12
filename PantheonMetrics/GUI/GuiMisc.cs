using Il2Cpp;
using Il2CppTMPro;
using PantheonMetrics.Data;
using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

namespace PantheonMetrics.GUI;

public class GuiMisc : GuiTogglePanelBase
{
  public GuiMisc(int x, int y, int width, int height) : base(x, y, width, height)
  {

  }

  public int ResetDeathLocation(int id, string str)
  {
    MetricsLocation.ResetLastKnownDeathPosistion();
    return 0;
  }

  public int PrintCorpseDirection(int id, string str)
  {
    var directions = MetricsLocation.GetDeathLocationDirections();
    MetricsLogging.LogMessageToInfoChat(directions);
    return 0;
  }
  public int GetLoc(int id, string str)
  {
    var pos = MetricsPlayer.PlayerPosition;
    return 0;
  }

  public void EnsureInitialialization()
  {
    if (_toggleTogglePanel == null)
      _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "Experience");
  }
  public string stringToEdit = "Hello World\nI've got 2 lines...";
  public override void Render()
  {
    int yOffset = 0;
    if (MetricsLocation.LastKnowDeathPosition != null)
    {
      //
      LabelM lastDeathLbl = new LabelM(0, _x, _y, 50, 20, null, new TextDefinition($"Death Loc", "White", 15) );
      ButtonM resetDeathLocBtn = new ButtonM(0, lastDeathLbl.GetLeftOf().x + 10, _y, 40, 20, ResetDeathLocation, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "Reset", "ResetDeathLoc");
      LabelM deathTextLbl = new LabelM(0, _x, lastDeathLbl.GetBelow().y + 2, _width, 20, null, new TextDefinition($"{MetricsLocation.LastKnowDeathPosition}", "White", 15));
      ButtonM directionsBtn = new ButtonM(0, _x , deathTextLbl.GetBelow().x +2, 60, 20, PrintCorpseDirection, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "Get Directions to Corpse", "CorpseDir");


      //var fromTextArea = UnityEngine.GUI.TextArea(rect, "StartingText", GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset));
      //stringToEdit = UnityEngine.GUI.TextArea(new Rect(600, 600, 200, 100), stringToEdit, 200);

      //stringToEdit = UnityEngine.GUI.TextField(new Rect(800, 800, 200, 20), stringToEdit, 200);

      //Some way to track the direction E.G. NW of current location

      lastDeathLbl.Render();
      resetDeathLocBtn.Render();
      deathTextLbl.Render();
      directionsBtn.Render();
      yOffset += 62;
    }
    ButtonM loc = new ButtonM(0, _x, _y + 100, 40, 20, GetLoc, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "GetLoc", "GetLoc");

    //stringToEdit = UnityEngine.GUI.TextArea(new Rect(600, 600, 200, 100), stringToEdit, 200);

    //var rect = new UnityEngine.Rect(600, 600, 100, 200);
    //stringToEdit = UnityEngine.GUI.TextField(rect, "StartingText", GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset));


    //UIPopupMenu.Instance.transform.position

    loc.Render();
    _toggleTogglePanel.Render();
    //Show 
    




    //var _igTimeGO = new GameObject("InGameTimeGO");
    //_igTimeGO.transform.SetParent(parentTransform, false);
    //var _igTimeTextMesh = _igTimeGO.AddComponent<TextMeshProUGUI>();
    //_igTimeTextMesh.color = Color.yellow;




  }
}
