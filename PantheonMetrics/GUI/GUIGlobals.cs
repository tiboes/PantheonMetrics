using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI;

public static class GUIGlobals
{
  private static GUIStyle buttonStyleActivated = null;
  private static GUIStyle buttonStyleStopped = null;
  private static GUIStyle buttonStyleStarted = null;
  private static GUIStyle buttonStyleResetExperience = null;
  private static GUIStyle boxStyle = null;
  private static GUIStyle panelStyleCentered = null;
  private static GUIStyle panelStyleInvisible = null;
  private static Texture2D transparentTexture = null;

  


  public static void EnsureInitialization()
  {
    if (buttonStyleActivated == null)
      CreateOrReloadStyles();
  }

  public static GUIStyle GetButtonStyle(ButtonStyleEnum style)
  {
    switch (style)
    {
      case ButtonStyleEnum.Box:
        return boxStyle;
      case ButtonStyleEnum.Activated:
        return buttonStyleActivated;
      case ButtonStyleEnum.Started:
        return buttonStyleStarted;
      case ButtonStyleEnum.Stopped:
        return buttonStyleStopped;
      case ButtonStyleEnum.Reset:
        return buttonStyleResetExperience;

      default: throw new ArgumentException("Unsupported style");
    }
  }

  public static GUIStyle GetPanelStyle(PanelStyleEnum style)
  {
    switch (style)
    {
      case PanelStyleEnum.Activated:
        return buttonStyleActivated;
      case PanelStyleEnum.Box:
        return boxStyle;
      case PanelStyleEnum.Centered:
        return panelStyleCentered;
      case PanelStyleEnum.Invisible:
        return panelStyleInvisible;


      default: throw new ArgumentException("Unsupported style");
    }
  }

  public static Texture2D GetTransparentBackground()
  {
    if (transparentTexture == null)
      transparentTexture = MakeTex(2, 2, new Color(0f, 0f, 0f, 0f));

    return transparentTexture;
  }


  public static void CreateOrReloadStyles()
  {
    buttonStyleActivated = new GUIStyle();

    buttonStyleActivated.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
    buttonStyleActivated.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 1f));
    buttonStyleActivated.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1f));
    buttonStyleActivated.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 1f));
    buttonStyleActivated.alignment = TextAnchor.MiddleLeft;

    buttonStyleResetExperience = new GUIStyle();

    buttonStyleResetExperience.normal.background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 0.5f));
    buttonStyleResetExperience.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleResetExperience.active.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    buttonStyleResetExperience.hover.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleResetExperience.alignment = TextAnchor.MiddleCenter;
    buttonStyleResetExperience.border = new RectOffset();

    buttonStyleStopped = new GUIStyle();

    buttonStyleStopped.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    //buttonStyleStarted.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStopped.active.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleStopped.hover.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    buttonStyleStopped.alignment = TextAnchor.MiddleCenter;
    buttonStyleStopped.border = new RectOffset();

    buttonStyleStarted = new GUIStyle();

    buttonStyleStarted.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
    //buttonStyleStopped.focused.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStarted.active.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
    buttonStyleStarted.hover.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
    buttonStyleStarted.alignment = TextAnchor.MiddleCenter;
    buttonStyleStarted.border = new RectOffset();




    boxStyle = new GUIStyle();
    boxStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
    boxStyle.margin.left = 2;

    panelStyleCentered = new GUIStyle();
    //panelStyleCentered.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.5f));
    //panelStyleCentered.margin.left = 2;
    panelStyleCentered.alignment = TextAnchor.MiddleCenter;

    if(transparentTexture != null)
      transparentTexture.MarkDirty();

    transparentTexture = MakeTex(2, 2,  new Color(0f, 0f, 0f, 0f));


    panelStyleInvisible = new GUIStyle();
    panelStyleInvisible.normal.background = transparentTexture;
    panelStyleInvisible.active.background = transparentTexture;
    panelStyleInvisible.hover.background = transparentTexture;
    panelStyleInvisible.focused.background = transparentTexture;
    
  }

  private static Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
    {
      pix[i] = col;
    }
    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();
    return result;
  }

}

public enum ButtonStyleEnum
{
  None = 0,
  Box = 1,
  Activated = 2,
  Started = 3,
  Stopped = 4,
  Reset = 5,
}

public enum PanelStyleEnum
{
  None = 0,
  Box = 1,
  Centered = 2,
  Activated = 3,
  Invisible = 4,
}
