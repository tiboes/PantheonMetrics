using Il2CppViNL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI.Objects;

public class PanelM : GUIElmentM
{
  private List<TextDefinition> TextDefinitions = new List<TextDefinition>();
  public void ResetTextDefinitions()
  {
    TextDefinitions.Clear();
  }


  public void AddText(TextDefinition textDefinition) => TextDefinitions.Add(textDefinition);

  public PanelM(int elementId, (int x, int y, int width, int height) xywh, GUIStyle style, string text = "") : base(elementId, xywh, style, text)
  {
  }

  public PanelM(int elementId, (int x, int y) xy, (int width, int height) wh, GUIStyle style, string text = "") : base(elementId, xy, wh, style, text)
  {
  }

  public PanelM(int elementId, int x, int y, int width, int height, GUIStyle style, string text = "") : base(elementId, x, y, width, height, style, text)
  {
  }

  public override void Render()
  {
    var r = new UnityEngine.Rect(X, Y, Width, Height);
    UnityEngine.GUI.Box(r, "");//, Style);
    if (TextDefinitions.Count == 0)
    {
      
      UnityEngine.GUI.Box(r, GUIUtils.GetGUIText($"{Text}", "Red", 10, true), Style);
      return;
    }
    int height = 0;
    foreach (var textDefinition in TextDefinitions) 
    {
      var rect = new UnityEngine.Rect(X, Y + height + 2, Width, textDefinition.Size + 2);

      var gS = new GUIStyle();
      gS.fontStyle = textDefinition.Bold ?  FontStyle.Bold : FontStyle.Normal;
      gS.alignment = textDefinition.TextAnchor;
      gS.fontSize = textDefinition.Size;
      //gS.normal.background = GUIGlobals.GetTransparentBackground();

      //UnityEngine.GUI.Box(rect, GUIUtils.Text(textDefinition));//, Style);
      UnityEngine.GUI.Label(rect, GUIUtils.TextT(textDefinition), gS);
      height += textDefinition.Size + 2;
    }
  }


}
