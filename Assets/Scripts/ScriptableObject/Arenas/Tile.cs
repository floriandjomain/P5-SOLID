using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region TileState

    public enum TileState
    {
        Vanilla,
        Hollow,
        Holy
    }

    #endregion
    
    [SerializeField] private Material healthyTile;
    [SerializeField] private Material midLifeTile;
    [SerializeField] private Material lastLifeTile;
    [SerializeField] private float startLifePoints;
    [SerializeField] private float currentLifePoints;
    [SerializeField] private int timer;
    [SerializeField] private GameObject cube;
    public event Action onDestroy;
    
    private void Awake()
    {
        UpdateColor();
        cube.transform.localScale = new Vector3(2f, 0.5f, 2f);
    }

    public void Damage(int damageAmount)
    {
        currentLifePoints-=damageAmount;

        UpdateColor();
    }

    private void UpdateColor()
    {
        if (currentLifePoints > startLifePoints / 2) return;
        
        if (currentLifePoints == startLifePoints / 2)
            cube.GetComponent<Renderer>().material.color = midLifeTile.color;
        else if (currentLifePoints == 1f && startLifePoints != 1f)
            cube.GetComponent<Renderer>().material.color = lastLifeTile.color;
        else if (currentLifePoints < 1f)
        {
            gameObject.SetActive(false);
            onDestroy?.Invoke();
        }
    }

    public void SetStartLife(int startLife)
    {
        startLifePoints   = startLife;
        currentLifePoints = startLife;
    }

    public void SetStartTimer(int _startTimer) => timer = _startTimer;

    public bool IsBroken() => currentLifePoints == 0;

    public void Break()
    {
        currentLifePoints = 0;
        UpdateColor();
    }

    public void TimerShot()
    {
        if (--timer <= 0) Damage(1);
    }

    public void AddActionToDeath(Action action)
    {
        onDestroy += action;
    }

    public float GetLife() => currentLifePoints;
}
