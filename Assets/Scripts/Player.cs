using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    public KeyBindings keyBindings;

    private void Update()
    {
        if (Input.GetKeyDown(keyBindings.up))
        {
            MovementManager.Instance.MoveUp(gameObject);
        }
        if (Input.GetKeyDown(keyBindings.right))
        {
            MovementManager.Instance.MoveRight(gameObject);
        }
        if (Input.GetKeyDown(keyBindings.down))
        {
            MovementManager.Instance.MoveDown(gameObject);
        }
        if (Input.GetKeyDown(keyBindings.left))
        {
            MovementManager.Instance.MoveLeft(gameObject);
        }
    }
}
