using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CoroutineManager : MonoBehaviour
{
    #region === Singleton ===
    public static CoroutineManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    /// Coroutines to execute
    [SerializeField] private List<GameCoroutine> _switchToSettingsMenuCoroutines;
    [SerializeField] private List<GameCoroutine> _switchToLobbyMenuCoroutines;
    [SerializeField] private List<GameCoroutine> _switchToGameViewCoroutines;

    /// Coroutine references
    [HideInInspector] public Coroutine _switchToSettingsMenuCoroutineRef;
    [HideInInspector] public Coroutine _switchToLobbyMenuCoroutineRef;
    [HideInInspector] public Coroutine _switchToGameViewCoroutineRef;

    /// Observer for UIManager
    public event Action<string> OnEndUICoroutine;

    bool _hasBeenInvok = false;

    //////////////////////////////
    /// Transition to Setting Menu
    //////////////////////////////

    public void SwitchToSettingsMenu()
    {
        /// Condition d'activation de la coroutine
        if (GetCoroutineActive())
        {
            _switchToSettingsMenuCoroutineRef = StartCoroutine(SwitchToSettingsMenuCoroutine());
        }
    }

    private IEnumerator SwitchToSettingsMenuCoroutine()
    {
        foreach (var coroutine in _switchToSettingsMenuCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine());
        }

        _switchToSettingsMenuCoroutineRef = null;
        OnEndUICoroutine.Invoke("GoToSettingsMenu");
    }

    //////////////////////////////
    /// Transition to Lobby Menu
    //////////////////////////////

    public void SwitchToLobbyMenu()
    {
        /// Condition d'activation de la coroutine
        if (GetCoroutineActive())
        {
            _switchToLobbyMenuCoroutineRef = StartCoroutine(SwitchToLobbyMenuCoroutine());
        }
    }

    private IEnumerator SwitchToLobbyMenuCoroutine()
    {
        foreach (var coroutine in _switchToLobbyMenuCoroutines)
        {
            yield return StartCoroutine(coroutine.ExecuteCoroutine());
        }

        _switchToLobbyMenuCoroutineRef = null;
        OnEndUICoroutine.Invoke("GoToLobbyMenu");
    }

    //////////////////////////////
    /// Transition to Game View
    //////////////////////////////

    public void SwitchToGameView()
    {
        /// Condition d'activation de la coroutine
        if (GetCoroutineActive())
        {
            _switchToGameViewCoroutineRef = StartCoroutine(SwitchToGameViewCoroutine());
        }
    }

    private IEnumerator SwitchToGameViewCoroutine()
    {
        foreach (var coroutine in _switchToGameViewCoroutines)
        {
            /// Active le jeu au moment où le fondu va se faire pour une meilleur impression
            if (coroutine.name.Contains("FadeOut"))
            {
                OnEndUICoroutine.Invoke("GoToGameView");
                _hasBeenInvok = true;
            }

            yield return StartCoroutine(coroutine.ExecuteCoroutine());
        }

        _switchToGameViewCoroutineRef = null;
        if(!_hasBeenInvok) { OnEndUICoroutine.Invoke("GoToGameView"); }
        else { _hasBeenInvok = false; }
    }

    ///////////////////
    /// Getter & Setter
    ///////////////////

    /// Return false if one of the coroutine is active
    private bool GetCoroutineActive()
    {
        return _switchToSettingsMenuCoroutineRef == null || _switchToLobbyMenuCoroutineRef == null || _switchToGameViewCoroutineRef == null;
    }
}
