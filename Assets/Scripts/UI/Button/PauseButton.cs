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

        ButtonManager.instance._pauseButton.SetActive(false);
    }

    private IEnumerator WaitEndOfCoroutine()
    {
        yield return CoroutineManager.instance._gameEndCoroutineRef;

        /// Activation de la nouvelle HUD après les coroutines
        ButtonManager.instance._startButton.SetActive(true);
    }
}