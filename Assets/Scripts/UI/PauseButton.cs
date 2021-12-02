using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Coroutine _waitEndOfCoroutineRef;

    public void OnButtonPress()
    {
        CoroutineManager.instance.GameEnd();

        _waitEndOfCoroutineRef = StartCoroutine(WaitEndOfCoroutine());

        UIManager.instance._pauseButton.SetActive(false);
    }

    private IEnumerator WaitEndOfCoroutine()
    {
        yield return CoroutineManager.instance._gameEndCoroutineRef;

        /// Activation de la nouvelle HUD apr�s les coroutines
        UIManager.instance._startButton.SetActive(true);
    }
}