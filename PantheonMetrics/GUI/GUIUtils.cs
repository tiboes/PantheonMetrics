using PantheonMetrics.GUI.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI;

public static class GUIUtils
{
  public static string GetGUIText(string message, string color, int textSize, bool isBold = false)
  {
    return $"  {GetBoldStartTag(isBold)}<color={color}><size={textSize}>{message}</size></color>{GetBoldEndTag(isBold)}";
  }
  public static string Text(TextDefinition textDef)
  {
    return $"  {GetBoldStartTag(textDef.Bold)}<color={textDef.Color}><size={textDef.Size}>{textDef.Text}</size></color>{GetBoldEndTag(textDef.Bold)}";
  }
  public static string TextT(TextDefinition textDef)
  {
    return $"  <color={textDef.Color}>{textDef.Text}</color>";
  }



  public static string GetBoldStartTag(bool isBold) => isBold ? "<b>" : "";
  public static string GetBoldEndTag(bool isBold) => isBold ? "<b>" : "";
}
