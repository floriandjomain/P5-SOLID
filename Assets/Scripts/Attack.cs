using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private Cooldown _attackCooldown;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_attackCooldown.IsCooldownDone())
            {
                Debug.Log("Attack!");
                _attackCooldown.StartCooldown();
            }
            else
            {
                Debug.Log("wait");
            }
        }
    }
}
