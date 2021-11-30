using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Fading _fading;

    public GameObject _startButton;

    public GameObject _pauseButton;

    public void OnButtonPress()
    {
        _fading.StartFadingInt();

        _startButton.SetActive(false);
        _pauseButton.SetActive(true);
    }
}
