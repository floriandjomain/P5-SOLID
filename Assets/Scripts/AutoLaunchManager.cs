using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AutoLaunchManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private GameObject _autoLaunchGameObject;
    [SerializeField] private CounterCoroutine _autoLaunchCounterCoroutine;
    [SerializeField] private IntVariable _minNumberPlayer;

    [SerializeField] private UnityEvent _onTimerEnds;

    private Coroutine _autoLaunchCoroutine;
    private Coroutine _counterCoroutine;

    private TMP_Text _autoLaunchText;

    private bool _isCoroutineRunning = false;

    private int _currentPlayerCount;
    private int _oldPlayerCount;

    private void OnEnable()
    {
        _autoLaunchText = _autoLaunchGameObject.transform.Find("Timer").GetComponent<TMP_Text>();
        _currentPlayerCount = _playerManager.GetCurrentPlayerNumber();
        _oldPlayerCount = _currentPlayerCount;
    }

    void Update()
    {
        if (_gameSettings.UseAutoLaunch)
        {
            _currentPlayerCount = _playerManager.GetCurrentPlayerNumber();

            if (_oldPlayerCount != _currentPlayerCount)
            {
                StopCounterCoroutine();

                if (_currentPlayerCount >= _minNumberPlayer.Value)
                {
                    StartCounterCoroutine();
                }

                _oldPlayerCount = _currentPlayerCount;
            }
        }
    }

    public void StartCounterCoroutine()
    {
        if (!_isCoroutineRunning)
        {
            Debug.Log("Start coroutine");

            _autoLaunchCoroutine = StartCoroutine(AutoLaunchCoroutine());

            _isCoroutineRunning = true;
        }
    }

    public void StopCounterCoroutine()
    {
        if (_isCoroutineRunning)
        {
            Debug.Log("Stop coroutine");

            StopCoroutine(_counterCoroutine);
            StopCoroutine(_autoLaunchCoroutine);

            _isCoroutineRunning = false;
            _autoLaunchText.text = "- -";
        }
    }

    private IEnumerator AutoLaunchCoroutine()
    {
        _counterCoroutine = StartCoroutine(_autoLaunchCounterCoroutine.ExecuteCoroutine());
        yield return _counterCoroutine;
        _onTimerEnds.Invoke();
    }
}
