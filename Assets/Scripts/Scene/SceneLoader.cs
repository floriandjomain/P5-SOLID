using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private List<Object> SceneList;

    void Awake()
    {
        foreach(var scene in SceneList)
        {
            //Debug.Log(SceneManager.GetActiveScene().name);
            if (scene.name != SceneManager.GetActiveScene().name)
            {
                //Debug.Log(SceneManager.GetActiveScene().name);
                //SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            }
        }
    }
}
