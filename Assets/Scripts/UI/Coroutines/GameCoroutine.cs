using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCoroutine : ScriptableObject
{
    public abstract IEnumerator ExecuteCoroutine();
}
