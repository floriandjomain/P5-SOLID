using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Coroutine/Fade")]
public class FadeCoroutine : GameCoroutine
{
    [SerializeField] private bool _isFadeIn;
    [SerializeField] private float _fadingSpeed;

    public override IEnumerator ExecuteCoroutine(params object[] parameters)
    {
        Image image = parameters[0] as Image;

        if (_isFadeIn)
        {
            while (image.color.a < 1)
            {
                image.color += new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (image.color.a > 0)
            {
                image.color -= new Color(0, 0, 0, _fadingSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
