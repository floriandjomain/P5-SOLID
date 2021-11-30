using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Fading _fading;

    public GameObject _pauseButton;

    public GameObject _startButton;

    public void OnButtonPress()
    {
        _fading.StartFadingOn();

        _pauseButton.SetActive(false);
        _startButton.SetActive(true);
    }
}