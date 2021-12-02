using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageValueSetter : MonoBehaviour
{
    [SerializeField] private ImageVariable _variable;

    void Start()
    {
        _variable.Value = GetComponent<Image>();
    }
}
