using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Written by Lyra

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public bool ControlsLocked = false;

    public GameObject Reticle;
    
    public GameObject TextBG;
    public TMP_Text HoverText;

    public GameObject ExitHint;
    public GameObject AudioStopHint;

    public ExaminableUI ExaminableUI;

    public TurntableRotate TurntableRotate;
    public AxisRotate AxisRotate;

    bool updateTextBGNextFrame = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Escape();
        HideText();
    }

    private void Update()
    {
        if (Input.GetKey("e")) { Escape(); }
        if (updateTextBGNextFrame) {
            updateTextBGNextFrame = false;
            TextBG.GetComponent<RectTransform>().sizeDelta = HoverText.GetRenderedValues() + new Vector2(10, 10);
        }
    }

    // Load mesh and material from an Examinable object, and open the respective UI.
    public void Examine(Examinable ex)
    {
        ControlsLocked = true;
        Reticle.SetActive(false);
        ExitHint.SetActive(true);
        ExaminableUI.gameObject.SetActive(true);
        ExaminableUI.Load(ex);
    }

    public void DisplayText(LookText lt)
    {
        HoverText.gameObject.SetActive(true);
        TextBG.gameObject.SetActive(true);
        HoverText.SetText(lt.Text);
        HoverText.fontSize = lt.FontSize;
        updateTextBGNextFrame = true;
    }

    public void HideText()
    {
        HoverText.gameObject.SetActive(false);
        TextBG.gameObject.SetActive(false);
    }

    // Close all active windows and release controls.
    // todo: "close window" button
    public void Escape()
    {
        ControlsLocked = false;
        Reticle.SetActive(true);
        ExaminableUI.Unload();
        ExaminableUI.gameObject.SetActive(false);
        ExitHint.SetActive(false);
        if (TurntableRotate != null) {
            TurntableRotate.End();
            TurntableRotate = null;
        }
        if (AxisRotate != null) {
            AxisRotate.End();
            AxisRotate = null;
        }
    }
}
