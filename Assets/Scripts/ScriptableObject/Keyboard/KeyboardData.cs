using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keyboard/Data")]
public class KeyboardData : ScriptableObject
{
    public string PlayerPseudo;
    public string DataName;
    public List<KeyData> Keys;

    public KeyCode GetKey(string keyName)
    {
        foreach (KeyData key in Keys)
        {
            if (key.name == keyName) return key.code;
        }

        return KeyCode.None;
    }
}

[System.Serializable]
public struct KeyData
{
    public string name;
    public KeyCode code;
}