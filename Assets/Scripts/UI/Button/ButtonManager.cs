using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    #region === Singleton ===
    public static ButtonManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject _startButton;
    public GameObject _pauseButton;
}
