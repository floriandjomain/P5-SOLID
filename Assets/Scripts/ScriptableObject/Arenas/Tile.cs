using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material healthyTile;
    [SerializeField] private Material midLifeTile;
    [SerializeField] private Material lastLifeTile;
    [SerializeField] private int startLifePoints;
    [SerializeField] private int currentLifePoints;
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
        if (currentLifePoints > startLifePoints / 2f)
            cube.GetComponent<Renderer>().material.color = healthyTile.color;
        else if (currentLifePoints == startLifePoints / 2f)
            cube.GetComponent<Renderer>().material.color = midLifeTile.color;
        else if (currentLifePoints == 1f)
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
        UpdateColor();
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

    public int GetStartLife() => startLifePoints;
    public int GetCurrentLife() => currentLifePoints;

    public static Tile Load(string _name, int _startLifePoints, int _currentLifePoints, int _timer)
    {
        Tile t = Instantiate(GameManager.Instance.GetTilePrefab(), GameManager.Instance.ArenaGO.transform, true);
        t.gameObject.SetActive(true);

        t.name = _name;
        t.startLifePoints = _startLifePoints;
        t.currentLifePoints = _currentLifePoints;
        t.timer = _timer;

        return t;
    }

    public int GetTimer() => timer;
}
