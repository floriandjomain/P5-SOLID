using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Coroutine/Counter")]
public class CounterCoroutine : GameCoroutine
{
    [SerializeField] private float _counterDelay;
    public override IEnumerator ExecuteCoroutine(params object[] parameters)
    {
        TMP_Text counterText = parameters[1] as TMP_Text;
        float counter = _counterDelay;

        counterText.gameObject.SetActive(true);

        Debug.Log("startCounter : " + parameters[1]);

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            counterText.text = counter.ToString("N0"); /// "N0"permet d'arrondir les float à l'unité près (int)
            yield return null;
        }

        counterText.gameObject.SetActive(false);
    }
}
