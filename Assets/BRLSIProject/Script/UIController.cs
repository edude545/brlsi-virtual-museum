using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Written by Lyra

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public bool ControlsLocked = false;

    public GameObject TextBG;
    public TMP_Text HoverText;

    public ExaminableUI ExaminableUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Escape();
    }

    private void Update()
    {
        if (Input.GetKey("e")) { Escape(); }
    }

    // Load mesh and material from an Examinable object, and open the respective UI.
    public void Examine(Examinable ex)
    {
        ControlsLocked = true;
        ExaminableUI.gameObject.SetActive(true);
        ExaminableUI.Load(ex);
    }

    public void DisplayText(string text)
    {
        HoverText.gameObject.SetActive(true);
        TextBG.gameObject.SetActive(true);
        HoverText.text = text;
        HoverText.ForceMeshUpdate();
        HoverText.SetText(text);
        Vector2 v = HoverText.GetRenderedValues() + new Vector2(10,10);
        TextBG.GetComponent<RectTransform>().sizeDelta = v;
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
        ExaminableUI.gameObject.SetActive(false);
    }
}
