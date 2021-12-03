using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVariableSetter<T> : VariableSetter
{
    /// Enregistre ici le scriptableObjet dont la valeur doit �tre d�finit
    [SerializeField] private GenericVariable<T> _variable;

    /// Le type du getComponent d�pend du type de la classe fille (exemple : TextVariableSetter : GenericVariableSetter<TMP_Text>)
    public override void Set()
    {
        _variable.Value = GetComponent<T>();
    }
}
