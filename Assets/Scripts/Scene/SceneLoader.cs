using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    List<Object> SceneList;

    void Awake()
    {
        foreach(var scene in SceneList)
        {
            if (scene.name != SceneManager.GetActiveScene().name)
            {
                //SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            }
        }
    }
}
