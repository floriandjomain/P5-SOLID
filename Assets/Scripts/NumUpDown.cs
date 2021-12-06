using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumUpDown : MonoBehaviour
{
    public GameSettings GameSettings;
    public TMP_InputField PlayerNumberInput;
    public int Increment = 1;
    public int MinValue = 0;
    public int MaxValue = int.MaxValue;

    public int SelectedIndex = 0;
    public string FieldName = "MaxPlayerNumber";

    private TMP_Text _placeholderText;
    private System.Reflection.FieldInfo _fieldInfo;

    public void Start()
    {
        _fieldInfo = GameSettings.GetType().GetField(FieldName);
        _placeholderText = PlayerNumberInput.placeholder.GetComponent<TMP_Text>();

        ChangePlaceholderText(GetValueField().ToString());

        PlayerNumberInput.onDeselect.AddListener(OnValueChange);
        PlayerNumberInput.characterValidation = TMP_InputField.CharacterValidation.Digit;
    }

    private int GetValueField()
    {
        return (int)_fieldInfo.GetValue(GameSettings);
    }

    private void SetValueField(object value)
    {
        _fieldInfo.SetValue(GameSettings, value);
    }

    private void OnValueChange(string value)
    {
        int intValue = int.Parse(value);
        // Verify the boundaries
        bool changed = false;
        if (intValue < MinValue)
        {
            intValue = MinValue;
            changed = true;
        }
        else if (intValue > MaxValue)
        {
            intValue = MaxValue;
            changed = true;
        }

        if (changed)
        {
            PlayerNumberInput.text = "";
            ChangePlaceholderText(intValue.ToString());
            SetValueField(intValue);
        }
    }
    

    public void IncrementNumber()
    {
        ReplaceInputText();
        int value = GetValueField() + Increment;
        if (value <= MaxValue)
        {
            SetValueField(value);
            ChangePlaceholderText(value.ToString());
        }
    }
    public void DecrementNumber()
    {
        ReplaceInputText();
        int value = GetValueField() - Increment;
        if (value >= MinValue)
        {
            SetValueField(value);
            ChangePlaceholderText(value.ToString());
        }
    }

    public void UpdateSettings()
    {
        SetValueField(int.Parse(PlayerNumberInput.text));
    }

    protected void ReplaceInputText()
    {
        if (PlayerNumberInput.text != "")
        {
            UpdateSettings();
            PlayerNumberInput.text = "";
        }
    }

    protected void ChangePlaceholderText(string text)
    {
        _placeholderText.text = text;
    }
}
