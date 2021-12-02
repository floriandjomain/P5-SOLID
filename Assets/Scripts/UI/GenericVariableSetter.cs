using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVariableSetter<T> : MonoBehaviour
{
    [SerializeField] private GenericVariableSetter<T> _variable;

    private void Start()
    {
        //
        //_variable.Value = GetComponent<T>();
    }
}
