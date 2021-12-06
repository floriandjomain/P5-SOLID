using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _channelField;
    [SerializeField] private UnityEvent _callback;

    public void VerifyInputs()
    {
        if(_channelField.text != "")
        {
            // Apply the settings
            // nb player
            // time to input
            // tile life points


            _callback.Invoke();
        }
    }
}
