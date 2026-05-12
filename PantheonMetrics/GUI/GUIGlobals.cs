using Il2CppSystem.Resources;
using Il2CppSystem.Xml.Serialization;
using MelonLoader.Utils;
using Mono.Cecil;
using PantheonMetrics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;

namespace PantheonMetrics.GUI;

public static class GUIGlobals
{
  private static GUIStyle buttonStyleActivated = null;
  private static GUIStyle buttonStyleStopped = null;
  private static GUIStyle buttonStyleStarted = null;
  private static GUIStyle buttonStyleResetExperience = null;
  private static GUIStyle boxStyle = null;
  private static GUIStyle panelStyleCentered = null;
  private static GUIStyle panelStyleSolid = null;
  private static GUIStyle panelStyleInvisible = null;


  private static GUIStyle buttonStyleOn = null;
  private static GUIStyle buttonStyleOff = null;
  private static GUIStyle buttonStyleLeftOn = null;
  private static GUIStyle buttonStyleLeftOff = null;
  private static GUIStyle buttonStyleSolid = null;


  private static Texture2D transparentTexture = null;

  private static Texture2D onTexture = null;
  private static Texture2D offTexture = null;
  private static Texture2D leftBtnOffTexture = null;
  private static Texture2D leftBtnOnTexture = null;




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
      case ButtonStyleEnum.Off:
        return buttonStyleOff;
      case ButtonStyleEnum.On:
        return buttonStyleOn;
      case ButtonStyleEnum.LeftOn:
        return buttonStyleLeftOn;
      case ButtonStyleEnum.LeftOff:
        return buttonStyleLeftOff;
      case ButtonStyleEnum.Solid:
        return buttonStyleSolid;




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
      case PanelStyleEnum.Solid:
        return panelStyleSolid;

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

    if (onTexture != null)
      onTexture.MarkDirty();

    if (offTexture != null)
      offTexture.MarkDirty();

    if (offTexture != null)
      offTexture.MarkDirty();
    if (leftBtnOffTexture != null)
      leftBtnOffTexture.MarkDirty();

    onTexture = LoadImageToTexture2d("blackBtn.png");//TODO skal slettes
    offTexture = LoadImageToTexture2d("blackBtn.png");//TODO skal slettes
    leftBtnOffTexture = LoadImageToTexture2d("blackBtn.png");
    leftBtnOnTexture = LoadImageToTexture2d("whiteBtn.png");


    buttonStyleOn = new GUIStyle();
    buttonStyleOn.normal.background = onTexture;
    buttonStyleOn.hover.background = offTexture;
    //buttonStyleOn.active.background = LoadImageToTexture2d("btnOff");
    //buttonStyleOn.hover.background = LoadImageToTexture2d("btnOff");

    buttonStyleOff = new GUIStyle();
    buttonStyleOff.normal.background = offTexture;
    buttonStyleOff.hover.background = onTexture;
    //buttonStyleOff.active.background = LoadImageToTexture2d("btnOff");
    //buttonStyleOff.hover.background = LoadImageToTexture2d("btnOff");

    buttonStyleLeftOn = new GUIStyle();
    buttonStyleLeftOn.normal.background = leftBtnOnTexture;
    buttonStyleLeftOn.hover.background = leftBtnOffTexture;
    buttonStyleLeftOn.normal.textColor = Color.black;
    buttonStyleLeftOn.hover.textColor = Color.white;
    buttonStyleLeftOn.alignment = TextAnchor.MiddleCenter;
    buttonStyleLeftOn.fontSize = 15;

    buttonStyleLeftOff = new GUIStyle();
    buttonStyleLeftOff.normal.background = leftBtnOffTexture;
    buttonStyleLeftOff.hover.background = leftBtnOnTexture;
    buttonStyleLeftOff.normal.textColor = Color.white;
    buttonStyleLeftOff.hover.textColor = Color.black;
    buttonStyleLeftOff.alignment = TextAnchor.MiddleCenter;
    buttonStyleLeftOff.fontSize = 15;




    panelStyleInvisible = new GUIStyle();
    panelStyleInvisible.normal.background = transparentTexture;
    panelStyleInvisible.active.background = transparentTexture;
    panelStyleInvisible.hover.background = transparentTexture;
    panelStyleInvisible.focused.background = transparentTexture;

    panelStyleSolid = new GUIStyle();
    panelStyleSolid.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f));

    buttonStyleSolid = new GUIStyle();
    buttonStyleSolid.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f));
    buttonStyleSolid.hover.background = MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 1f));

  }

  /// <summary>
  /// Uses the path of the mod directory + provided imageName for our mod to load images from into Texture2D objects.
  /// </summary>
  /// <param name="imageName"></param>
  /// <returns></returns>
  public static Texture2D LoadImageToTexture2d(string imageName)
  {
    Assembly currentAssembly = Assembly.GetExecutingAssembly();
    string[] resources = currentAssembly.GetManifestResourceNames();

    
    var imageLocation = Path.Combine(Path.GetDirectoryName(MelonEnvironment.ModsDirectory) ?? "", "Mods/MetricsResources", "", imageName);
    var imageAsBytes = File.ReadAllBytes(imageLocation);

    var image = new Texture2D(2, 2);
    
    unsafe
    {
      var intPtr = UnityEngine.Object.MarshalledUnityObject.MarshalNotNull(image);

      fixed (byte* ptr = imageAsBytes)
      {
        var managedSpanWrapper = new ManagedSpanWrapper(ptr, imageAsBytes.Length);

        ImageConversion.LoadImage_Injected(intPtr, ref managedSpanWrapper, false);
      }
    }

    return image;
  }

  public static Texture2D MakeTex(int width, int height, Color col)
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
  On = 6,
  Off = 7,
  LeftOn = 8,
  LeftOff = 9,
  Solid = 10,
}

public enum PanelStyleEnum
{
  None = 0,
  Box = 1,
  Centered = 2,
  Activated = 3,
  Invisible = 4,
  Solid = 5,
}
