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

    public void StartGame()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
    }
}
