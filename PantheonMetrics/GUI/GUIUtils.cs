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


  public static string GetBoldStartTag(bool isBold) => isBold ? "<b>" : "";
  public static string GetBoldEndTag(bool isBold) => isBold ? "<b>" : "";
}
