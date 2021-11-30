using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    IEnumerator _fadingOn;
    IEnumerator _fadingInt;

    public float _fadingSpeed = 1.5f;

    public Image _fadingImage;

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
