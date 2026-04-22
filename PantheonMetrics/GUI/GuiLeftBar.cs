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

public static class GuiLeftBar
{
  //private static Texture2D backgroundTexture = null;
  


  private static bool _isOn = true;
  private static ToggleBarStatesEnum _toggleState = ToggleBarStatesEnum.Hide;
  private static int _x = 0;
  private static int _y = 50;
  private static int _width = 15;
  private static int _toggleBarHeight = (_width * 15) ;
  private static int _panelDefaultWidth = 300;

  private static ButtonM _onOffButton;
  private static ButtonM _resetButton;
  private static ButtonM _toggleBarButton;

  private static PanelM _dpsTogglePanel;
  private static PanelM _encounterTogglePanel;

  private static GuiExperience _experience;
  private static GuiMisc _misc;
  private static GuiDPS _dps; 
  private static GuiEncounterConfig _encounterConfig;

  public static void Reset()
  {
    //IsOn = false;
  }

  public static int PanelWidth => _panelDefaultWidth;
  

  public static void Render()
  {
    GUIGlobals.EnsureInitialization();

    RenderOnOffButton();
    RenderResetButton();
    RenderToggleBarButton();
  }

  public static void InitializeRenderObjects()
  {
    GUIGlobals.CreateOrReloadStyles();
    ResetStyles();
    //mabye mark something as dirty
  }

  private static void RenderResetButton()
  {
    if (_isOn)
    {
      _resetButton.Render();
    }
  }

  private static void RenderToggleBarButton()
  {
    if (_isOn)
    {
      _toggleBarButton.Render();
      if (_toggleState != ToggleBarStatesEnum.Hide)
      {
        //DO Something
        if (_toggleState == ToggleBarStatesEnum.DPS)
        {
          _dps.Render();
        }
        if (_toggleState == ToggleBarStatesEnum.Encounter)
        {
          _encounterConfig.Render();
          //render encounter
          //Enable logging to console
          //By Participant
          //By Ability
          //By Damage Type

        }
        if (_toggleState == ToggleBarStatesEnum.EXP)
        {
          _experience.Render();
        }

        if (_toggleState == ToggleBarStatesEnum.MISC)
        {
          _misc.Render();
        }

      }
    }
  }

  



  private static void RenderOnOffButton()
  {
    EnsureInitialialization();

    if (_isOn)
    {
      _onOffButton.SetGUIStyle(GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started));
      _onOffButton.Text = "";
    }
    else
    {
      _onOffButton.SetGUIStyle(GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped));
      _onOffButton.Text = "V";
    }

    _onOffButton.Render();

    //Everything follows the on/off button


  }

  private static void EnsureInitialialization()
  {
    if (_onOffButton == null)
      _onOffButton = new ButtonM(0, _x, _y, _width, _width, ToggleOnOff, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started), "v", "off");
    if (_resetButton == null)
      _resetButton = new ButtonM(1, _onOffButton.GetBelow(), (_width, _width), Reset, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset), "R", "");
    if (_toggleBarButton == null)
      _toggleBarButton = new ButtonM(2, _resetButton.GetBelow(), (_width, _toggleBarHeight), ToggleBar, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Activated), "", "");
    if (_experience == null)
    { 
      _experience = new GuiExperience(_onOffButton.GetLeftOf().x, _onOffButton.GetLeftOf().y, PanelWidth, (_onOffButton.Height + _resetButton.Height + _toggleBarButton.Height));
      _experience.EnsureInitialialization();
    }
    if (_misc == null)
    { 
      _misc = new GuiMisc(_onOffButton.GetLeftOf().x, _onOffButton.GetLeftOf().y, PanelWidth, (_onOffButton.Height + _resetButton.Height + _toggleBarButton.Height)); 
      _misc.EnsureInitialialization();
    }
    if (_dps == null)
    {
      _dps= new GuiDPS(_onOffButton.GetLeftOf().x, _onOffButton.GetLeftOf().y, PanelWidth, (_onOffButton.Height + _resetButton.Height + _toggleBarButton.Height));
      _dps.EnsureInitialialization();
    }
    if (_encounterConfig == null)
    {
      _encounterConfig = new GuiEncounterConfig(_onOffButton.GetLeftOf().x, _onOffButton.GetLeftOf().y, PanelWidth, (_onOffButton.Height + _resetButton.Height + _toggleBarButton.Height));
      _encounterConfig.EnsureInitialialization();
    }

  }
  public static void ResetStyles()
  {
    _onOffButton.SetGUIStyle(GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started));
    _resetButton.SetGUIStyle(GUIGlobals.GetButtonStyle(ButtonStyleEnum.Reset));
    _toggleBarButton.SetGUIStyle(GUIGlobals.GetButtonStyle(ButtonStyleEnum.Activated));
  }



  public static int ToggleBar(int id, string value)
  {
    switch (_toggleState)
    {
      case ToggleBarStatesEnum.Hide:
        _toggleState = ToggleBarStatesEnum.DPS;
        break;
      case ToggleBarStatesEnum.DPS:
        _toggleState = ToggleBarStatesEnum.Encounter;
        break;
      case ToggleBarStatesEnum.Encounter:
        _toggleState = ToggleBarStatesEnum.EXP;
        break;
      case ToggleBarStatesEnum.EXP:
        _toggleState = ToggleBarStatesEnum.MISC;
        break;
      case ToggleBarStatesEnum.MISC:
        _toggleState = ToggleBarStatesEnum.Hide;
        break;
    }

    MetricsLogging.LogMessageToInfoChat($"ToggleBar switched to: {_toggleState}.");
    return 0;
  }
  public static int Reset(int id, string value)
  {
    MetricsLogging.LogMessageToInfoChat($"Reset pressed.");
    return 0;
  }

  public static int ToggleOnOff(int id, string value)
  {
    if (_isOn)
    {
      MetricsLogging.LogMessageToInfoChat($"Turning Off.");
      //TODO Disable stuff
      MetricsConfiguration.ExperienceMetricEnabled = false;
      MetricsConfiguration.CombatTrackingEnabled = false;


    } else
    {
      MetricsLogging.LogMessageToInfoChat($"Turning On.");
      MetricsConfiguration.ExperienceMetricEnabled = true;
      MetricsConfiguration.CombatTrackingEnabled = true;
      //TODO Enable stuff
    }
    _isOn =!_isOn;

    return 0;
  }

  

}
public enum ToggleBarStatesEnum
{
  Hide = 0,
  DPS = 1,
  Encounter = 2,
  EXP = 3,
  MISC = 4,
}
