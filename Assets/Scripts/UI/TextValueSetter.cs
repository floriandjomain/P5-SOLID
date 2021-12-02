using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextValueSetter : GenericVariableSetter<TMP_Text>
{
    [SerializeField] private TextVariable _variable;

    void Start()
    {
        _variable.Value = GetComponent<TMP_Text>();
    }
}
