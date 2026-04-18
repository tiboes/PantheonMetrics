using PantheonMetrics.Data;
using HarmonyLib;
using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppTMPro;
using MelonLoader;

using PantheonMetrics.Hooks;
using PantheonMetrics.Logics;
using PantheonMetrics.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Il2CppSystem.Xml.XmlTextReaderImpl;

namespace PantheonMetrics.GUI.Objects;

public class ButtonM : GUIElmentM
{
  private Func<int, string, int> _buttonClick;
  private object _clickInput;

  

  public ButtonM(int buttonId, int x, int y, int width, int height, Func<int, string, int> buttonClick, GUIStyle style, string text, object clickInput = null ) : base(buttonId, x, y, width, height, style, text)
  {
    _buttonClick = buttonClick;
    _clickInput = clickInput;

  }

  public ButtonM(int buttonId, (int x, int y) xy, (int width, int height) wh, Func<int, string, int> buttonClick, GUIStyle style, string text, object clickInput = null) : base(buttonId, xy, wh, style, text)
  {
    _buttonClick = buttonClick;
    _clickInput = clickInput;
  }

  public ButtonM(int buttonId, (int x, int y, int width, int height) xywh, Func<int, string, int> buttonClick, GUIStyle style, string text, object clickInput = null) : base(buttonId, xywh, style, text)
  {
    _buttonClick = buttonClick;
    _clickInput = clickInput;
  }

  public override void Render()
  {
    Render(_buttonClick, _clickInput);
  }

  private void Render(Func<int, string, int> buttonClick, object clickInput = null)
  {
    var rect = new UnityEngine.Rect(X, Y, Width, Height);
    if (UnityEngine.GUI.Button(rect, Text, Style))
    {
      if (clickInput != null && clickInput is string str)
        buttonClick(ElementId, $"Clicked: {str}");
    }
  }

}
