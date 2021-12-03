using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableSetterManager : MonoBehaviour
{
    void Start()
    {
        foreach(var variableSetter in FindObjectsOfType<VariableSetter>(true)) /// Le (true) permet de rechercher les gameObjet désactivé
        {
            variableSetter.Set();
        }
    }
}
