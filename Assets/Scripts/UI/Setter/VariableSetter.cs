using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Permet d'�tre recherch� par le manager VariableSetterManager (impossible de faire une recherche sur une class g�n�rique)
public abstract class VariableSetter : MonoBehaviour
{
    public abstract void Set();
}
