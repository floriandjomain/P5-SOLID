using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Coroutine/Wait")]
public class WaitCoroutine : GameCoroutine
{
    [SerializeField] private float _delay;

    public override IEnumerator ExecuteCoroutine()
    {
        yield return new WaitForSeconds(_delay);
    }
}
