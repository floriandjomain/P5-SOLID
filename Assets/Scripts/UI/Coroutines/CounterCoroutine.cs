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

        float floatToDisplay;

        while (counter > -0.5f) /// -0.5f pour afficher le 0 plus longtemps
        {
            counter -= Time.deltaTime;
            floatToDisplay = counter + 0.5f; /// Amelioration d'affichage
            counterText.text = floatToDisplay.ToString("N0"); /// "N0"permet d'arrondir les float � l'unit� pr�s (int)
            yield return null;
        }

        counterText.gameObject.SetActive(false);
    }
}
