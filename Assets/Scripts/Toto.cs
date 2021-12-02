using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toto : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private GameObject LobbyUI;
    [SerializeField] private GameObject camera;

    public void OnClick()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        LobbyUI.SetActive(false);
        camera.SetActive(false);
    }
}
