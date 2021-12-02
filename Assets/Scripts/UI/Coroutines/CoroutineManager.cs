using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoroutineManager : MonoBehaviour
{
    #region === Singleton ===
    public static CoroutineManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] private TMP_Text _textCounter;
    [SerializeField] private Image _fadingImage;
    [SerializeField] private Image _otherImage; // test

    [Tooltip("Liste de coroutines a activer au gameStart event")]
    [SerializeField] private List<GameCoroutine> _startCoroutines;

    [Tooltip("Liste de coroutines a activer au gameEnd event")]
    [SerializeField] private List<GameCoroutine> _endCoroutines;

    [HideInInspector] 
    public Coroutine _gameStartCoroutineRef;
    [HideInInspector] 
    public Coroutine _gameEndCoroutineRef;

    private void Start()
    {
        /// Test area
        //GameStart();
    }

    public void GameStart()
    {
        /// Condition d'activation de la coroutine
        if (_gameEndCoroutineRef == null && _gameStartCoroutineRef == null)
        {
            _gameStartCoroutineRef = StartCoroutine(GameStartCoroutine());
        }
    }

    private IEnumerator GameStartCoroutine()
    {
        foreach (var coroutine in _startCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine(_fadingImage, _textCounter));
        }

        _gameStartCoroutineRef = null;
    }

    public void GameEnd()
    {
        /// Condition d'activation de la coroutine
        if (_gameStartCoroutineRef == null && _gameEndCoroutineRef == null)
        {
            _gameEndCoroutineRef = StartCoroutine(GameEndCoroutine());
        }
    }

    private IEnumerator GameEndCoroutine()
    {
        foreach (var coroutine in _endCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine(_fadingImage, _textCounter));
        }

        _gameEndCoroutineRef = null;
    }
}
