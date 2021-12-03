using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Coroutine/Fade")]
public class FadeCoroutine : GameCoroutine
{
    [SerializeField] private bool _isFadeIn;
    [SerializeField] private float _fadingSpeed;

    [SerializeField] private ImageVariable _fadeImage;

    public override IEnumerator ExecuteCoroutine()
    {
        //Image image = parameters[0] as Image;

        if (_isFadeIn)
        {
            while (_fadeImage.Value.color.a < 1)
            {
                _fadeImage.Value.color += new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (_fadeImage.Value.color.a > 0)
            {
                _fadeImage.Value.color -= new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
