using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Reflection;

[CustomEditor(typeof(NumUpDown))]
public class NumUpDownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NumUpDown numUpDown = (NumUpDown)target;

        numUpDown.GameSettings = EditorGUILayout.ObjectField("Game Settings", numUpDown.GameSettings, typeof(GameSettings), true) as GameSettings;
        if (numUpDown.GameSettings != null)
        {
            // try to display the available int in the class we selected
            FieldInfo[] fieldInfos = numUpDown.GameSettings.GetType().GetFields();
            List<string> fields = new List<string> { "" };
            foreach (FieldInfo info in fieldInfos)
            {
                if (info.FieldType == typeof(int))
                {
                    fields.Add(info.Name);
                }
            }

            numUpDown.SelectedIndex = EditorGUILayout.Popup("Field Name", numUpDown.SelectedIndex, fields.ToArray());
            numUpDown.FieldName = fields[numUpDown.SelectedIndex];
        }

        EditorGUILayout.Space();

        numUpDown.NumberInput = EditorGUILayout.ObjectField("Number Input", numUpDown.NumberInput, typeof(TMP_InputField), true) as TMP_InputField;

        EditorGUILayout.Space();

        numUpDown.Increment = EditorGUILayout.IntField("Increment", numUpDown.Increment);
        numUpDown.MinValue = EditorGUILayout.IntField("Min Value", numUpDown.MinValue);
        numUpDown.MaxValue = EditorGUILayout.IntField("Max Value", numUpDown.MaxValue);
    }
}
