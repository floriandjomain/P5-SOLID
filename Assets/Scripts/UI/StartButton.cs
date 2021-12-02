using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject _startButton;
    public GameObject _pauseButton;

    public void OnButtonPress()
    {
        CoroutineManager.instance.GameStart();

        _startButton.SetActive(false);
        _pauseButton.SetActive(true);
    }
}
