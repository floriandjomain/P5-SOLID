using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesManager : MonoBehaviour
{
    private static ScenesManager _instance;
    public static ScenesManager Instance
    {
        get => _instance;
    }

    [SerializeField] private List<string> twitchScenes;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        _instance = this;
    }

    public void StartTwitchBot()
    {
        foreach(string sceneName in twitchScenes)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void StopTwitchBot()
    {
        foreach (string sceneName in twitchScenes)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public void UnloadLevelIfLoaded()
    {
        bool isLoaded = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Level")
            {
                isLoaded = true;
            }
        }

        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync("Level");
        }        
    }

    public IEnumerator StartGame()
    {
        yield return SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        yield return GameManager.Instance.SetUp();
        yield return GameManager.Instance.StartGame();
    }
}
