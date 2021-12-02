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
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] private TMP_Text _textCounter;
    [SerializeField] private Image _fadingImage;

    [Tooltip("Liste de coroutines a activer au gameStart event")]
    [SerializeField] private List<GameCoroutine> _startCoroutines;

    [Tooltip("Liste de coroutines a activer au gameEnd event")]
    [SerializeField] private List<GameCoroutine> _endCoroutines;

    private void Start()
    {
        /// Test area
        //GameStart();
    }

    public void GameStart()
    {
        StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        foreach (var coroutine in _startCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine(_fadingImage, _textCounter));
        }
    }

    public void GameEnd()
    {
        StartCoroutine(GameEndCoroutine());
    }

    private IEnumerator GameEndCoroutine()
    {
        foreach (var coroutine in _endCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine(_fadingImage, _textCounter));
        }
    }

    ////////////////////////////
    /// SECTION OBSELETE /!\ ///
    ////////////////////////////

    IEnumerator _fadingOn;
    IEnumerator _fadingInt;

    private float _fadingSpeed = 1.5f;

    /// Active fondu noir
    public void StartFadingOn()
    {
        StopAllCoroutines();
        _fadingOn = FadingOn();
        StartCoroutine(_fadingOn);
    }

    private IEnumerator FadingOn()
    {
        while(_fadingImage.color.a < 1)
        {
            _fadingImage.color += new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// Desactive fondu noir
    public void StartFadingInt()
    {
        StopAllCoroutines();
        _fadingInt = FadingInt();
        StartCoroutine(_fadingInt);
    }

    private IEnumerator FadingInt()
    {
        while (_fadingImage.color.a > 0)
        {
            _fadingImage.color -= new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
