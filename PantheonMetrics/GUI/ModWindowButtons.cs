using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonMetrics.GUI;

public static class ModWindowButtons
{
  private static ButtonM _infoButton;
  private static ButtonM _encounterButton;
  private static ButtonM _experienceButton;
  private static ButtonM _locationButton;
  //private static GuiDPS _dpsPanel;
  private static GuiEncounter _encounterPanel;
  private static GuiExperience _experiencePanel;
  private static GuiInfo _infoPanel;
  private static GuiLocation _locationPanel;


  private static int infoButtonOffsetY = 0;
  private static int encounterButtonOffsetY = 50;
  private static int experienceButtonOffsetY = 100;
  private static int locationButtonOffsetY = 150;

  private static int buttonOffsetX = -30;
  private static int buttonWidth = 80;
  private static int buttonHeight = 30;


  private static int _x = 0;
  private static int _y = 0;
  private static bool _showInfo = true;
  private static bool _showEncounter = false;
  private static bool _showExperience = false;
  private static bool _showLocation = false;

  public static void Render()
  {
    if (!UpdateLocation())
      return;
    EnsureInitialialization();

    _infoButton.Render();
    _encounterButton.Render();
    _experienceButton.Render();
    _locationButton.Render();
    //_dpsPanel.Render(_x + 30, _y);

    if (_showInfo)
      _infoPanel.Render(_x + 50, _y + 20);
    if (_showEncounter )
      _encounterPanel.Render(_x + 50, _y + 20);
    if (_showExperience )
      _experiencePanel.Render(_x + 50, _y + 20);
    if (_showLocation)
      _locationPanel.Render(_x + 50, _y + 20);
  }

  private static bool UpdateLocation()
  {
    var xyCoords = ModWindow.WindowLocationTopLeftOrigin();
    if (!xyCoords.visible)
      return false;
    _x = (int)xyCoords.x;
    _y = (int)xyCoords.y;

    return true;
  }
  public static void InitializeRenderObjects()
  {
    GUIGlobals.CreateOrReloadStyles();
    ResetStyles();
    //mabye mark something as dirty
  }

  private static void EnsureInitialialization()
  {

    if (_infoButton == null)
      _infoButton = new ButtonM(0, _x + buttonOffsetX, _y + infoButtonOffsetY, buttonWidth, buttonHeight, ShowInfo, GUIGlobals.GetButtonStyle((_showInfo? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)), "Information", "info");

    if (_encounterButton == null)
      _encounterButton = new ButtonM(0, _x + buttonOffsetX, _y + encounterButtonOffsetY, buttonWidth, buttonHeight, ShowEncounter, GUIGlobals.GetButtonStyle((_showEncounter ? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)), "Encounter", "encounter");

    if (_experienceButton == null)
      _experienceButton = new ButtonM(0, _x + buttonOffsetX, _y + experienceButtonOffsetY, buttonWidth, buttonHeight, ShowExperience, GUIGlobals.GetButtonStyle((_showExperience? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)), "Experience", "experience");
    
    if (_locationButton == null)
      _locationButton = new ButtonM(0, _x + buttonOffsetX, _y + locationButtonOffsetY, buttonWidth, buttonHeight, ShowLocation, GUIGlobals.GetButtonStyle((_showLocation? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)), "Location", "location");



    //if (_dpsPanel == null)
    //  _dpsPanel = new GuiDPS(_x + 30, _y, ModWindow.Width - 70, ModWindow.Height - 70);

    if (_infoPanel == null)
      _infoPanel = new GuiInfo(_x + 50, _y + 20, ModWindow.Width - 70, ModWindow.Height - 70);


    if (_encounterPanel == null)
      _encounterPanel = new GuiEncounter(_x + 50, _y + 20, ModWindow.Width - 70, ModWindow.Height - 70);

    if (_experiencePanel == null)
      _experiencePanel = new GuiExperience(_x + 50, _y + 20, ModWindow.Width - 70, ModWindow.Height - 70);

    if (_locationPanel == null)
      _locationPanel = new GuiLocation(_x + 50, _y + 20, ModWindow.Width - 70, ModWindow.Height - 70);


    if (_x == 0 && _y == 0)
      return;

    _infoButton.UpdateXY(_x + buttonOffsetX, _y + infoButtonOffsetY);
    _encounterButton.UpdateXY(_x + buttonOffsetX, _y + encounterButtonOffsetY);
    _experienceButton.UpdateXY(_x + buttonOffsetX, _y + experienceButtonOffsetY);
    _locationButton.UpdateXY(_x + buttonOffsetX, _y + locationButtonOffsetY);
    //_dpsPanel.EnsureInitialialization();
    _infoPanel.EnsureInitialialization();
    _encounterPanel.EnsureInitialialization();
    _experiencePanel.EnsureInitialialization();
  }

  public static void ResetStyles()
  {
    if (_infoButton != null)
      _infoButton.SetGUIStyle(GUIGlobals.GetButtonStyle((_showInfo ? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)));
    if (_encounterButton != null)
      _encounterButton.SetGUIStyle(GUIGlobals.GetButtonStyle((_showEncounter ? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)));
    if (_experienceButton != null)
      _experienceButton.SetGUIStyle(GUIGlobals.GetButtonStyle((_showExperience ? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)));
    if (_locationButton != null)
      _locationButton.SetGUIStyle(GUIGlobals.GetButtonStyle((_showLocation? ButtonStyleEnum.LeftOn : ButtonStyleEnum.LeftOff)));


  }
  public static int ShowInfo(int id, string value)
  {
    TurnOffAllRendering();
    _showInfo = true;
    ResetStyles();
    return 0;
  }
  public static int ShowEncounter(int id, string value)
  {
    TurnOffAllRendering();
    _showEncounter = true;
    ResetStyles();
    return 0;
  }

  public static int ShowExperience(int id, string value)
  {
    TurnOffAllRendering();
    _showExperience = true;
    ResetStyles();
    return 0;
  }
  public static int ShowLocation(int id, string value)
  {
    TurnOffAllRendering();
    _showLocation= true;
    ResetStyles();
    return 0;
  }

  public static void TurnOffAllRendering()
  {
    _showInfo = false;
    _showEncounter = false;
    _showExperience = false;
    _showLocation = false;
  }


}
