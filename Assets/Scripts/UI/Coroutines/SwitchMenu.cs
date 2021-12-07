using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Coroutine/Switch")]
public class SwitchMenu : GameCoroutine
{
    [SerializeField] private string _menuToSwitch;


    public override IEnumerator ExecuteCoroutine()
    {
        UIManager.Instance.SwitchMenu(_menuToSwitch);
        yield return null;
    }
}
