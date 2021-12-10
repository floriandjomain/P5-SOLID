using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private CounterCoroutine _backLobbyCounterCoroutine;

    [SerializeField] private UnityEvent _onTimerEnds;


    private Coroutine _backLobbyCoroutine;
    private Coroutine _counterCoroutine;

    private bool _isCoroutineRunning = false;

    private void OnEnable()
    {
        Debug.Log("Enable");
        // Start coroutine
        if (!_isCoroutineRunning)
        {
            _backLobbyCoroutine = StartCoroutine(BackLobbyCoroutine());
            _isCoroutineRunning = true;
        }
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        // Stop coroutine if running
        if (_isCoroutineRunning)
        {
            StopCoroutine(_backLobbyCoroutine);
            StopCoroutine(_counterCoroutine);
            _isCoroutineRunning = false;
        }
    }

    private IEnumerator BackLobbyCoroutine()
    {
        _counterCoroutine = StartCoroutine(_backLobbyCounterCoroutine.ExecuteCoroutine());
        yield return _counterCoroutine;
        _isCoroutineRunning = false;
        _onTimerEnds.Invoke();
    }
}
