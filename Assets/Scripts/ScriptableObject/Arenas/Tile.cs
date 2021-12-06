using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material healthyTile;
    [SerializeField] private Material midLifeTile;
    [SerializeField] private Material lastLifeTile;
    [SerializeField] private int startLifePoints;
    [SerializeField] private int currentLifePoints;
    [SerializeField] private int startTimer;
    [SerializeField] private int currentTimer;
    [SerializeField] private bool isHollow;
    [SerializeField] private GameObject cube;
    public event Action onDestroy;
    private void Awake()
    {
        cube.GetComponent<Renderer>().material.color = healthyTile.color;
        cube.transform.localScale = new Vector3(2f, 0.5f, 2f);
    }

    public void Damage(int damageAmount)
    {
        currentLifePoints-=damageAmount;

        OnDamage();
    }

    private void OnDamage()
    {
        if (currentLifePoints > startLifePoints / 2) return;
        
        if (currentLifePoints == 1)
            cube.GetComponent<Renderer>().material.color = lastLifeTile.color;
        else if (currentLifePoints == startLifePoints / 2)
            cube.GetComponent<Renderer>().material.color = midLifeTile.color;
        else if (currentLifePoints <= 0)
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

    public void SetStartTimer(int _startTimer)
    {
        startTimer   = _startTimer;
        currentTimer = _startTimer;
    }

    public bool IsBroken() => isHollow || currentLifePoints == 0;

    public void SwitchHollow()
    {
        isHollow = !isHollow;

        if (isHollow)
        {
            gameObject.SetActive(false);
            cube.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            gameObject.SetActive(true);
            OnDamage();
        }
    }

    public void Break()
    {
        currentLifePoints = 0;
        OnDamage();
    }

    public void TimerShot()
    {
        currentTimer--;

        if (currentTimer > 0)
        {
            Damage(1);
            currentTimer = startTimer;
        }
    }

    public void AddActionToDeath(Action action)
    {
        onDestroy += action;
    }
}
