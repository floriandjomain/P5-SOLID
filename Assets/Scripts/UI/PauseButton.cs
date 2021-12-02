using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject _pauseButton;
    public GameObject _startButton;

    public void OnButtonPress()
    {
        CoroutineManager.instance.GameEnd();

        _pauseButton.SetActive(false);
        _startButton.SetActive(true);
    }
}