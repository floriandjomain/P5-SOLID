using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string _bootstrapSceneName;

    private void Awake()
    {
        bool isLoaded = false;

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(SceneManager.GetSceneAt(i).name == _bootstrapSceneName)
            {
                isLoaded = true;
            }
        }

        if (!isLoaded)
        {
            SceneManager.LoadScene(_bootstrapSceneName);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
