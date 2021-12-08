using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Coroutine/Counter")]
public class CounterCoroutine : GameCoroutine
{
    [SerializeField] private TextVariable _counterText;
    [SerializeField] private float _counterDelay;
    [SerializeField] private bool _usePrecision;

    public override IEnumerator ExecuteCoroutine()
    {
        //TMP_Text counterText = parameters[1] as TMP_Text;
        float counter = _counterDelay;

        _counterText.Value.gameObject.SetActive(true);

        float floatToDisplay;

        while (counter > -0.5f) /// -0.5f pour afficher le 0 plus longtemps
        {
            counter -= Time.deltaTime;
            floatToDisplay = counter + 0.5f; /// Amelioration d'affichage
            _counterText.Value.text = floatToDisplay.ToString((_usePrecision ? "N1" : "N0")); /// "N0"permet d'arrondir les float à l'unité près (int)

            yield return null;
        }

        _counterText.Value.gameObject.SetActive(false);
    }

    public void SetCounterDelay(float delay)
    {
        _counterDelay = delay;
    }
}
