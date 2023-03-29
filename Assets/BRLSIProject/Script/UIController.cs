using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by Lyra

public class UIController : MonoBehaviour
{
    public bool ControlsLocked = false;
    public GameObject ExaminableUI;

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
        ExaminableUI.SetActive(true);
        ex.LoadModel(ExaminableUI.transform.GetChild(0).gameObject);
    }

    // Close all active windows and release controls.
    // todo: "close window" button
    public void Escape()
    {
        ControlsLocked = false;
        ExaminableUI.SetActive(false);
    }
}
