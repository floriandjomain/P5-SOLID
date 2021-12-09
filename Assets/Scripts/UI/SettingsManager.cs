using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _channelField;
    [SerializeField] private UnityEvent _eventAfterVerification;
    [SerializeField] private GameSettings _gameSettings;
    [Space(10)]
    [SerializeField] private CounterCoroutine _gameCoroutineTime;
    [SerializeField] private CounterCoroutine _autoLaunchCoroutineTime;
    [Space(10)]
    [SerializeField] private Toggle _toggleKeepPlayer;
    [Space(10)]
    [SerializeField] private NumUpDown _numUpDownPlayerNumber;
    [SerializeField] private NumUpDown _numUpDownCommandInputTime;
    [SerializeField] private NumUpDown _numUpDownTileLifePoints;
    [Space(10)]
    [SerializeField] private NumUpDown _numUpDownAutoLaunchTime;
    [SerializeField] private Toggle _toggleAutoLaunch;
    [Space(10)]
    [SerializeField] private StringVariable _channelName;


    private void Start()
    {
        _toggleAutoLaunch.isOn = _gameSettings.UseAutoLaunch;
        _numUpDownAutoLaunchTime.gameObject.SetActive(_gameSettings.UseAutoLaunch);
    }

    public void VerifyInputs()
    {
        if(_channelField.text != "")
        {
            // Get the InputValues
            int inputValuePlayerNumber = _numUpDownPlayerNumber.GetInputValue();
            int inputValueCommandTime = _numUpDownCommandInputTime.GetInputValue();
            int inputValueTileLife = _numUpDownTileLifePoints.GetInputValue();

            // Apply the settings
            _gameSettings.MaxPlayerNumber = inputValuePlayerNumber;
            _gameSettings.CommandInputTime = inputValueCommandTime;
            _gameSettings.TileMaxLifePoints = inputValueTileLife;

            // Change the value of the inputs
            _numUpDownPlayerNumber.ChangePlaceholderText(inputValuePlayerNumber.ToString());
            _numUpDownCommandInputTime.ChangePlaceholderText(inputValueCommandTime.ToString());
            _numUpDownTileLifePoints.ChangePlaceholderText(inputValueTileLife.ToString());

            _gameCoroutineTime.SetCounterDelay(inputValueCommandTime);

            _channelName.Value = _channelField.text;

            _gameSettings.KeepPlayersAfterFinish = _toggleKeepPlayer.isOn;

            // Auto launch
            _gameSettings.UseAutoLaunch = _toggleAutoLaunch.isOn;
            if (_gameSettings.UseAutoLaunch) {
                int autoLaunchTime = _numUpDownAutoLaunchTime.GetInputValue();
                _gameSettings.AutoLaunchTime = autoLaunchTime;
                _autoLaunchCoroutineTime.SetCounterDelay(autoLaunchTime);
            }

            _eventAfterVerification.Invoke();
        }
    }
}
