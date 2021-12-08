using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumUpDown : MonoBehaviour
{
    public GameSettings GameSettings;
    public TMP_InputField NumberInput;
    public int Increment = 1;
    public int MinValue = 0;
    public int MaxValue = int.MaxValue;

    public int SelectedIndex = 0;
    public string FieldName = "MaxPlayerNumber";

    private TMP_Text _placeholderText;
    private System.Reflection.FieldInfo _fieldInfo;
    private int _currentValue;

    public void Start()
    {
        _fieldInfo = GameSettings.GetType().GetField(FieldName);
        _placeholderText = NumberInput.placeholder.GetComponent<TMP_Text>();

        _currentValue = GetValueField();

        ChangePlaceholderText(_currentValue.ToString());

        NumberInput.onDeselect.AddListener(OnDeselect);
        NumberInput.characterValidation = TMP_InputField.CharacterValidation.Digit;
    }

    private int GetValueField()
    {
        return (int)_fieldInfo.GetValue(GameSettings);
    }

    private void SetValueField(object value)
    {
        _fieldInfo.SetValue(GameSettings, value);
    }

    private void OnDeselect(string value)
    {
        int intValue;
        if (int.TryParse(value, out intValue))
        {
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
                NumberInput.text = "";
                ChangePlaceholderText(intValue.ToString());      
            }

            _currentValue = intValue;
        }
    }

    public int GetInputValue()
    {
        string returnValue;
        if(NumberInput.text == "")
        {
            // Return the placeholder
            returnValue = _placeholderText.text;
        }
        else
        {
            // Return the input text
            returnValue = NumberInput.text;
        }
        return int.Parse(returnValue);
    }
    

    public void IncrementNumber()
    {
        RemoveInputText();
        int value = _currentValue + Increment;
        if (value <= MaxValue)
        {
            _currentValue = value;
            ChangePlaceholderText(value.ToString());
        }
    }

    public void DecrementNumber()
    {
        RemoveInputText();
        int value = _currentValue - Increment;
        if (value >= MinValue)
        {
            _currentValue = value;
            ChangePlaceholderText(value.ToString());
        }
    }

    private void RemoveInputText()
    {
        if (NumberInput.text != "") NumberInput.text = "";
    }

    public void ChangePlaceholderText(string text)
    {
        NumberInput.text = "";
        _placeholderText.text = text;
    }
}
