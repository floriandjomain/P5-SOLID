using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Coroutine _waitEndOfCoroutineRef;

    public void OnButtonPress()
    {
        CoroutineManager.instance.GameStart();

        _waitEndOfCoroutineRef = StartCoroutine(WaitEndOfCoroutine());

        ButtonManager.instance._startButton.SetActive(false);
    }

    private IEnumerator WaitEndOfCoroutine()
    {
        yield return CoroutineManager.instance._gameStartCoroutineRef;

        /// Activation de la nouvelle HUD après les coroutines
        ButtonManager.instance._pauseButton.SetActive(true);
    }
}
