using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVariableSetter<T> : VariableSetter
{
    [SerializeField] private GenericVariable<T> _variable;

    /// Le type du getComponent dépend du type de la classe fille (exemple : TextVariableSetter : GenericVariableSetter<TMP_Text>)
    public override void Set()
    {
        _variable.Value = GetComponent<T>();
    }
}
