using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesManager : MonoBehaviour
{
    private static ScenesManager _instance;
    public static ScenesManager Instance
    {
        get => _instance;
    }

    [SerializeField] private List<string> _twitchScenes;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        _instance = this;
    }

    public void StartTwitchBot()
    {
        foreach(string sceneName in _twitchScenes)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    public void StopTwitchBot()
    {
        foreach (string sceneName in _twitchScenes)
        {
            UnloadIfLoaded(sceneName);
            // SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public void UnloadIfLoaded(string sceneName)
    {
        bool isLoaded = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                isLoaded = true;
            }
        }

        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public void UnloadLevelIfLoaded()
    {
        UnloadIfLoaded("Level");     
    }

    public IEnumerator StartGame()
    {
        yield return SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("SaveSystem", LoadSceneMode.Additive);
        yield return GameManager.Instance.SetUp();
        yield return GameManager.Instance.StartGame();
    }
}
