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

public class GuiEncounterConfig : GuiTogglePanelBase
{

  public GuiEncounterConfig(int x, int y, int width, int height) : base(x, y, width, height)
  { 
  }

  public void EnsureInitialialization()
  {
    if (_toggleTogglePanel == null)
      _toggleTogglePanel = new PanelM(0, _x, _y, _width, _height, GUIGlobals.GetButtonStyle(ButtonStyleEnum.Box), "Experience");
  }

  public int ShowHidePressed(int id, string str)
  {
    MetricsCombat.ShowEncounterInChat = !MetricsCombat.ShowEncounterInChat;
    return 0;
  }

  public int ShowAttackerPressed(int id, string str)
  {
    MetricsCombat.ShowAttackerBreakdown = !MetricsCombat.ShowAttackerBreakdown;
    return 0;
  }

  public int ShowAbilityPressed(int id, string str)
  {
    MetricsCombat.ShowAbilityBreakdown = !MetricsCombat.ShowAbilityBreakdown;
    return 0;
  }

  public int ShowDamageTypePressed(int id, string str)
  {
    MetricsCombat.ShowDamageTypeBreakdown = !MetricsCombat.ShowDamageTypeBreakdown;
    return 0;
  }

  public int ShowMitigatedPressed(int id, string str)
  {
    MetricsCombat.ShowMitigatedDamage = !MetricsCombat.ShowMitigatedDamage;
    return 0;
  }

  public override void Render()
  {
    _toggleTogglePanel.ResetTextDefinitions();
    _toggleTogglePanel.SetGUIStyle(GUIGlobals.GetPanelStyle(PanelStyleEnum.Box));
    _toggleTogglePanel.AddText(new TextDefinition("Encounter Configuration", "Red", 20, true));

    var guiStyleShowEncounter = MetricsCombat.ShowEncounterInChat ? GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started) : GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped);
    var showEncounterButtonText = MetricsCombat.ShowEncounterInChat ? "Encounter in chat: Active" : "Encounter in chat: Inactive";

    ButtonM showHideButton = new ButtonM(0, _toggleTogglePanel.X, _toggleTogglePanel.Y + 25, _toggleTogglePanel.Width, 25, ShowHidePressed, guiStyleShowEncounter, showEncounterButtonText, "ShowHideEncounter");

    if(MetricsCombat.ShowEncounterInChat)
    {
      var guiStyleShowAttacker = MetricsCombat.ShowAttackerBreakdown ? GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started) : GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped);
      ButtonM showAttackerBreakdownButton = new ButtonM(0, showHideButton.GetBelow().x, showHideButton.GetBelow().y + 5, 15 , 15 , ShowAttackerPressed, guiStyleShowAttacker, "", "ShowHideAttacker");
      LabelM showAttackerBreakdownLabel = new LabelM(0, showAttackerBreakdownButton.GetLeftOf().x + 10, showAttackerBreakdownButton.GetLeftOf().y, _toggleTogglePanel.Width - 30, 15, null, new TextDefinition("Display Attacker Breakdown", "White", 15));

      var guiStyleShowAbility = MetricsCombat.ShowAbilityBreakdown ? GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started) : GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped);
      ButtonM showAbilityBreakdownButton = new ButtonM(0, showAttackerBreakdownButton.GetBelow().x, showAttackerBreakdownButton.GetBelow().y + 5, 15, 15, ShowAbilityPressed, guiStyleShowAbility, "", "ShowHideAbility");
      LabelM showAbilityBreakdownLabel = new LabelM(0, showAbilityBreakdownButton.GetLeftOf().x + 10, showAbilityBreakdownButton.GetLeftOf().y, _toggleTogglePanel.Width - 30, 15, null, new TextDefinition("Display Ability Breakdown", "White", 15));

      var guiStyleShowType = MetricsCombat.ShowDamageTypeBreakdown ? GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started) : GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped);
      ButtonM showTypeBreakdownButton = new ButtonM(0, showAbilityBreakdownButton.GetBelow().x, showAbilityBreakdownButton.GetBelow().y + 5, 15, 15, ShowDamageTypePressed, guiStyleShowType, "", "ShowHideDamageType");
      LabelM showTypeBreakdownLabel = new LabelM(0, showTypeBreakdownButton.GetLeftOf().x + 10, showTypeBreakdownButton.GetLeftOf().y, _toggleTogglePanel.Width - 30, 15, null, new TextDefinition("Display Damage Type Breakdown", "White", 15));

      //ShowMitigatedDamage
      var guiStyleShowMitigated = MetricsCombat.ShowMitigatedDamage ? GUIGlobals.GetButtonStyle(ButtonStyleEnum.Started) : GUIGlobals.GetButtonStyle(ButtonStyleEnum.Stopped);
      ButtonM showMitigatedButton = new ButtonM(0, showTypeBreakdownButton.GetBelow().x, showTypeBreakdownButton.GetBelow().y + 5, 15, 15, ShowMitigatedPressed, guiStyleShowMitigated, "", "ShowHideMitigated");
      LabelM showMitigatedLabel = new LabelM(0, showMitigatedButton.GetLeftOf().x + 10, showMitigatedButton.GetLeftOf().y, _toggleTogglePanel.Width - 30, 15, null, new TextDefinition("Display Mitigated Damage", "White", 15));

      showAttackerBreakdownButton.Render();
      showAttackerBreakdownLabel.Render();
      showAbilityBreakdownButton.Render();
      showAbilityBreakdownLabel.Render();
      showTypeBreakdownButton.Render();
      showTypeBreakdownLabel.Render();
      showMitigatedButton.Render();
      showMitigatedLabel.Render();

    }
    showHideButton.Render();
    _toggleTogglePanel.Render();

  }
}
