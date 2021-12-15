using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Save : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectToSave;

    void Start()
    {
        SaveObject(_gameObjectToSave);
    }

    private void GetData()
    {

    }

    private void SaveObject(GameObject gameObject)
    {
        Debug.Log(JsonUtility.ToJson(gameObject));
    }
}
