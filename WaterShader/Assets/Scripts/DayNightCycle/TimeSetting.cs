using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TimeSetting
{
    // thoughts:

    // have list of values with x length
    // corresponding list of ints as time hours
    public List<int> TimeStamps;
    public List<float> Values;

    // custom lerp function that knows how to build an animation curve out of these values
    // that function / property I guess returns the Animation curve

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TimeSetting))]
public class TimeSettingDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        //var indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 300, position.height);
        var unitRect = new Rect(position.x + 305, position.y, 500, position.height);
     //   var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("TimeStamps"));
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("Values"));

        // Set indent back to what it was
       //    EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif