using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommand : MonoBehaviour
{
    public event System.Action _moveForward;
    public event System.Action _moveBackward;
    public event System.Action _moveRight;
    public event System.Action _moveLeft;

    // Update is called once per frame
    void Update()
    {
        
    }
}
