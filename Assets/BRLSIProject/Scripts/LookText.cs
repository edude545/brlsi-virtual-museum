using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Behavior implemented in KBMPlayer
public class LookText : MonoBehaviour
{
    //[LargeTextField]
    [TextArea]
    public string Text = "Sample Text";
    public int FontSize = 18;
    public float MaxLookDistance = 10f;
}
