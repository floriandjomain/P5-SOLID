using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event System.Action<string> _moveEvent;

    void Update()
    {
        bool _isMoving = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);

        if (_isMoving)
        {
            _moveEvent.Invoke("direction");
        }
    }
}
