using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material _healthyTile;
    [SerializeField] private Material _midLifeTile;
    [SerializeField] private Material _lastLifeTile;
    [SerializeField] private int _startLifePoints;
    [SerializeField] private int _currentLifePoints;
    [SerializeField] private int _timer;
    [SerializeField] private GameObject _cube;

    public event Action onDestroy;

    public int Timer { get => _timer; set => _timer = value; }
    public int StartLifePoints { get => _startLifePoints; set => _startLifePoints = value; }
    public int CurrentLifePoints { get => _currentLifePoints; set => _currentLifePoints = value; }


    public bool IsBroken() => _currentLifePoints == 0;
    public void SetStartTimer(int _startTimer) => _timer = _startTimer;


    private void Awake()
    {
        UpdateColor();
        _cube.transform.localScale = new Vector3(2f, 0.5f, 2f);
    }

    public void Damage(int damageAmount)
    {
        _currentLifePoints -= damageAmount;
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (_currentLifePoints > _startLifePoints / 2f)
            _cube.GetComponent<Renderer>().material.color = _healthyTile.color;
        else if (_currentLifePoints == _startLifePoints / 2f)
            _cube.GetComponent<Renderer>().material.color = _midLifeTile.color;
        else if (_currentLifePoints == 1f)
            _cube.GetComponent<Renderer>().material.color = _lastLifeTile.color;
        else if (_currentLifePoints < 1f)
        {
            gameObject.SetActive(false);
            onDestroy?.Invoke();
        }
    }

    public void SetStartLife(int startLife)
    {
        _startLifePoints   = startLife;
        _currentLifePoints = startLife;
        UpdateColor();
    }

    public void Break()
    {
        _currentLifePoints = 0;
        UpdateColor();
    }

    public void TimerShot()
    {
        if (--_timer <= 0) Damage(1);
    }

    public void AddActionToDeath(Action action)
    {
        onDestroy += action;
    }

    public static Tile Load(string name, int startLifePoints, int currentLifePoints, int timer)
    {
        Tile t = Instantiate(GameManager.Instance.GetTilePrefab(), GameManager.Instance.ArenaGO.transform, true);
        t.gameObject.SetActive(true);

        t.name = name;
        t.StartLifePoints = startLifePoints;
        t.CurrentLifePoints = currentLifePoints;
        t.Timer = timer;

        return t;
    }
}
