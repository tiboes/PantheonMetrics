using Il2CppViNL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI.Objects;

public class LabelM : GUIElmentM
{
  private TextDefinition _textDefinition;


  

  public LabelM(int elementId, (int x, int y, int width, int height) xywh, GUIStyle style, TextDefinition textDefinition) : base(elementId, xywh, style, textDefinition.Text)
  {
    _textDefinition = textDefinition;
  }

  public LabelM(int elementId, (int x, int y) xy, (int width, int height) wh, GUIStyle style, TextDefinition textDefinition) : base(elementId, xy, wh, style, textDefinition.Text)
  {
    _textDefinition = textDefinition;
  }

  public LabelM(int elementId, int x, int y, int width, int height, GUIStyle style, TextDefinition textDefinition) : base(elementId, x, y, width, height, style, textDefinition.Text)
  {
    _textDefinition = textDefinition;
  }

  public override void Render()
  {
    var rect = new UnityEngine.Rect(X, Y, Width, Height);

    var gS = new GUIStyle();
    gS.fontStyle = _textDefinition.Bold ? FontStyle.Bold : FontStyle.Normal;
    gS.alignment = _textDefinition.TextAnchor;
    gS.fontSize = _textDefinition.Size;
    //gS.normal.background = GUIGlobals.GetTransparentBackground();

    //UnityEngine.GUI.Box(rect, GUIUtils.Text(textDefinition));//, Style);
    UnityEngine.GUI.Label(rect, GUIUtils.TextT(_textDefinition), gS);
  }


}
