using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _channelField;
    [SerializeField] private UnityEvent _callback;
    [SerializeField] private GameSettings _gameSettings;
    [Space(10)]
    [SerializeField] private NumUpDown _numUpDownPlayerNumber;
    [SerializeField] private NumUpDown _numUpDownCommandInputTime;
    [SerializeField] private NumUpDown _numUpDownTileLifePoints;
    [Space(10)]
    [SerializeField] private StringVariable _channelName;

    public void VerifyInputs()
    {
        if(_channelField.text != "")
        {
            // Apply the settings
            _gameSettings.MaxPlayerNumber = _numUpDownPlayerNumber.GetInputValue();
            _gameSettings.CommandInputTime = _numUpDownCommandInputTime.GetInputValue();
            _gameSettings.TileMaxLifePoints = _numUpDownTileLifePoints.GetInputValue();

            _channelName.Value = _channelField.text;

            _callback.Invoke();
        }
    }
}
