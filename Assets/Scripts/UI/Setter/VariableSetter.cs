using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Permet d'être recherché par le manager VariableSetterManager (impossible de faire une recherche sur une class générique)
public abstract class VariableSetter : MonoBehaviour
{
    public abstract void Set();
}
