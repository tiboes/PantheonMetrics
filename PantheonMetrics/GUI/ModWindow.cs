using Il2Cpp;
using Il2CppTMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace PantheonMetrics.GUI;

public class ModWindow : MonoBehaviour
{
  // Holds the panel
  private static UIWindowPanel gUiWindowPanel = null;
  public static bool Show = false;
  public static int Width { get; set; }
  public static int Height { get; set; }

  public static (bool visible, float x, float y) WindowLocationTopLeftOrigin()
  {
    if (gUiWindowPanel == null || !Show)
      return (false, 0, 0);

    var currentPosition = gUiWindowPanel.transform.position;

    var height = Screen.height;
    var y = currentPosition.y;

    return (true, currentPosition.x, (height - y));
  }

  public void DisplayPanel(string panelName, Transform midPanel, Vector2 panelSize)
  {
    // Setup the general panel parameters
    GameObject gameObject = new GameObject(panelName);
    // Add the panel to the Mid, this ensures we get rendered
    gameObject.transform.SetParent(midPanel);
    gameObject.layer = Layers.UI;

    // Add the necessary component for a panel
    CanvasRenderer canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
    CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
    UIDraggable uiDraggable = gameObject.AddComponent<UIDraggable>();
    RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
    gUiWindowPanel = gameObject.AddComponent<UIWindowPanel>();

    // Block Raycasts to work around wonky click detection on the close button due other UI elements overlapping the close button image
    // I am not going to spend time making all my TextMesh's layout perfectly for this mod so block raycasts instead
    canvasGroup.blocksRaycasts = true;

    // Setup the Window Panel
    uiDraggable._windowPanel = gUiWindowPanel;
    gUiWindowPanel.CanvasGroup = canvasGroup;
    gUiWindowPanel._displayName = panelName;

    // Setup the default position of the panel and its general parameters
    rectTransform.sizeDelta = panelSize;
    Width = (int)panelSize.x;
    Height = (int)panelSize.y;
    rectTransform.pivot = new Vector2(0, 1);
    rectTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);

    // Add the MANDATORY elements to a panel, the compilor will not error if you don't do this but nothing will work
    BuildCloseButtonAndBackground(midPanel, gameObject);

    // Add in my text Meshs that will ultimate display the data in the panel
    //BuildTextMeshs();

    // Load the panel and display it
    gUiWindowPanel.Show();
    Show = true;
  }
  private void BuildCloseButtonAndBackground(Transform midPanel, GameObject gameObject)
  {
    // Source for copying button and backgrounds
    UITutorialPopup tutorialPopup = midPanel.GetComponentInChildren<UITutorialPopup>();
    Transform tutorialButton = tutorialPopup.transform.GetChild(0);

    // Initialise the background for the new panel (MANDATORY)
    UnityEngine.UI.Image imageToCopy = tutorialPopup.GetComponent<UnityEngine.UI.Image>();
    var image = gameObject.AddComponent<UnityEngine.UI.Image>();
    image.type = UnityEngine.UI.Image.Type.Sliced;
    image.sprite = imageToCopy.sprite;

    // Initialise the close button of the panel (MANDATORY)
    var closeButton = GameObject.Instantiate(tutorialButton, tutorialButton.transform.position, tutorialButton.transform.rotation, gUiWindowPanel.transform);
    var closeButtonRect = closeButton.GetComponent<RectTransform>();
    closeButtonRect.sizeDelta = new Vector2(30, 30);
    closeButtonRect.anchoredPosition = new Vector2(-13.5f, -12); // Tiny size, top right corner, this ruins the box detection though
    closeButtonRect.pivot = new Vector2(0f, 0f);

    // Initialise on click behaviour of the close button
    var buttonComponent = closeButton.GetComponent<UnityEngine.UI.Button>();
    buttonComponent.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
    buttonComponent.onClick.RemoveAllListeners();
    buttonComponent.onClick.AddCall(new InvokableCall(new Action(() =>
    {
      // Actually unloads the panel, not hide
      gUiWindowPanel.Hide();
      Show = false;
    })));
    // Make clicking sound
    buttonComponent.onClick.AddCall(new InvokableCall(new Action(() =>
    {
      closeButton.GetComponent<UI_Audio_Function>().Play_UI_Generic_Click();
    })));
  }
  // Called by the /clock command to show the clock panel again if the user closes it
  public void ShowWindow()
  {
    gUiWindowPanel.Show();
    Show = true;
  }
}