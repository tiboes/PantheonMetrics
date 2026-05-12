using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI.Objects;

public class LeftBarPanel : GUIElmentM
{
  private List<TextDefinition> TextDefinitions = new List<TextDefinition>();
  private GUIStyle style;
  private GUIStyle headerStyle;

  public LeftBarPanel(int elementId, int x, int y, int width, int height, GUIStyle style, string headerTxt) : base(elementId, x, y, width, height, style, headerTxt)
  {
    this.style = style;
  }

  public void ResetTextDefinitions()
  {
    TextDefinitions.Clear();
  }

  public void AddText(TextDefinition textDefinition) => TextDefinitions.Add(textDefinition);

  public override void Render()
  {
    if (headerStyle == null)
    {
      headerStyle = new GUIStyle();
      headerStyle.fontSize = 20;
      headerStyle.normal.textColor = Color.blue;
      headerStyle.alignment = TextAnchor.MiddleCenter;
    }
    

    var r = new UnityEngine.Rect(X, Y, Width, Height);
    UnityEngine.GUI.Box(r, "", Style);

    var headerRect = new UnityEngine.Rect(X, Y, Width, 25);
    UnityEngine.GUI.Label(headerRect, Text, headerStyle);

    int height = 306;
    foreach (var textDefinition in TextDefinitions)
    {
      var rect = new UnityEngine.Rect(X, Y + height + 2, Width, textDefinition.Size + 2);

      var gS = new GUIStyle();
      gS.fontStyle = textDefinition.Bold ? FontStyle.Bold : FontStyle.Normal;
      gS.alignment = textDefinition.TextAnchor;
      gS.fontSize = textDefinition.Size;
      //gS.normal.background = GUIGlobals.GetTransparentBackground();

      //UnityEngine.GUI.Box(rect, GUIUtils.Text(textDefinition));//, Style);
      UnityEngine.GUI.Label(rect, GUIUtils.TextT(textDefinition), gS);
      height += textDefinition.Size + 2;
    }


  }

}
