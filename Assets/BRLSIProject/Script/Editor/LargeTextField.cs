using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Code from https://gamedev.stackexchange.com/questions/201214/large-text-in-unity-inspector
// WIP

public class LargeTextField : PropertyAttribute
{
}

[CustomPropertyDrawer(typeof(LargeTextField))]
public class LargeTextFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var style = new GUIStyle();
        style.fontSize = 40;
        EditorGUI.LabelField(position, label.text, property.stringValue, style);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 45;
    }
}