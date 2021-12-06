using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumUpDown : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private TMP_InputField _playerNumberInput;
    [SerializeField] private int _increment = 1;
    [SerializeField] private string fieldName = "MaxPlayerNumber";
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = int.MaxValue;

    private TMP_Text _placeholderText;
    private System.Reflection.FieldInfo fieldInfo;

    public void Start()
    {
        fieldInfo = _gameSettings.GetType().GetField(fieldName);
        _placeholderText = _playerNumberInput.placeholder.GetComponent<TMP_Text>();

        ChangePlaceholderText(GetValueField().ToString());

        _playerNumberInput.onValueChanged.AddListener(OnValueChange);
    }

    private int GetValueField()
    {
        return (int)fieldInfo.GetValue(_gameSettings);
    }

    private void SetValueField(object value)
    {
        fieldInfo.SetValue(_gameSettings, value);
    }

    private void OnValueChange(string value)
    {
        // Verify that it's a number
        if(!int.TryParse(value, out _))
        {
            _playerNumberInput.text = "";
        }
    }

    public void IncrementNumber()
    {
        ReplaceInputText();
        int value = GetValueField() + _increment;
        if (value <= maxValue)
        {
            SetValueField(value);
            ChangePlaceholderText(value.ToString());
        }
    }
    public void DecrementNumber()
    {
        ReplaceInputText();
        int value = GetValueField() - _increment;
        if (value >= minValue)
        {
            SetValueField(value);
            ChangePlaceholderText(value.ToString());
        }
    }

    public void UpdateSettings()
    {
        SetValueField(int.Parse(_playerNumberInput.text));
    }

    protected void ReplaceInputText()
    {
        if (_playerNumberInput.text != "")
        {
            UpdateSettings();
            _playerNumberInput.text = "";
        }
    }

    protected void ChangePlaceholderText(string text)
    {
        _placeholderText.text = text;
    }
}
