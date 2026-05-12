using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PantheonMetrics.GUI.Objects;

public class GUIElmentM
{
  private int _elementId;
  private int _x;
  private int _y;
  private int _width;
  private int _height;
  private string _text;
  private GUIStyle _style;

  //private Func<object, object> _buttonClick;

  public int X => _x;
  public int Y => _y;
  public int Width => _width;
  public int Height
  {
    get
    {
      return _height;
    }
    set
    {
      _height = value;
    }
  }

  public void UpdateXY(int x, int y)
  {
    _x = x;
    _y = y;
  }

  public int ElementId => _elementId; 
  
  public string Text
  {
    get => _text;
    set => _text = value;
  }

  public GUIStyle Style
  {
    get => _style;
    set => _style = value;
  }

  public GUIElmentM(int elementId, int x, int y, int width, int height, GUIStyle style, string text = "")
  {
    _elementId = elementId;
    _x = x;
    _y = y;
    _width = width;
    _height = height;
    _text = text;
    _style = style;
  }

  public GUIElmentM(int elementId, (int x, int y) xy, (int width, int height) wh, GUIStyle style, string text = "") : this(elementId, xy.x, xy.y, wh.width, wh.height, style, text) { }
  public GUIElmentM(int elementId, (int x, int y, int width, int height) xywh, GUIStyle style, string text = "") : this(elementId, xywh.x, xywh.y, xywh.width, xywh.height, style, text) { }

  public void SetGUIStyle(GUIStyle style)
  {
    _style = style;
  }

  public virtual void Render()
  {

  }


  public (int x, int y) GetXY()
  {
    return (Width, Height);
  }

  public (int width, int height) GetWidthHeight()
  {
    return (Width, Height);
  }

  public (int x, int y) GetLeftOf()
  {
    return (_x + _width, _y);
  }

  public (int x, int y) GetBelow()
  {
    return (_x, _y + _height);
  }

  public (int x, int y) GetLeftOfAndBelow()
  {
    return (_x + _width, _y + _height);
  }

  public (int x, int y, int width, int height) GetXYWH()
  {
    return (_x, _y, _width, _height);
  }
}
