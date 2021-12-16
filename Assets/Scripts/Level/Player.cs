using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2Int _position;
    [SerializeField] private Vector2Int _nextPosition;
    [SerializeField] private GameObject _capsule;
    [SerializeField] private bool _isAlive;
    [SerializeField] private bool _willUTurn;
    [SerializeField] private float _height;
    [SerializeField] private bool _isMoving;

    private float _playTime;
    private bool _uTurnMax;

    public float PlayTime { get => _playTime; set => _playTime = value; }
    public bool IsMoving { get => _isMoving; }
    public bool WillUTurn { get => _willUTurn; }
    public Vector2Int Position { get => _position; set => _position = value; }
    public Vector2Int NextPosition { get => _nextPosition; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }


    public Vector3 GetCapsulePos() => _capsule.transform.position;

    // Start is called before the first frame update
    private void Start()
    {
        _isAlive = true;
        _willUTurn = false;
        gameObject.transform.position = new Vector3(0f, -10000000f, 0f);
    }

    private IEnumerator CoUTurn(Vector2 start, Vector2 falseEnd)
    {
        Vector3 worldStart = Util.ConvertPositionToWorld(start, _height);
        Vector3 worldFalseEnd = Util.ConvertPositionToWorld(falseEnd, _height);
        Vector3 colPosition = (worldStart * (_uTurnMax?2:1) + worldFalseEnd) / (_uTurnMax?3:2);
        
        yield return StartCoroutine(MoveFromTo(worldStart, colPosition, _playTime/2));
        yield return StartCoroutine(MoveFromTo(colPosition, worldStart, _playTime/2));
    }

    private IEnumerator MoveFromTo(Vector3 start, Vector3 dest)
    {
        yield return MoveFromTo(start, dest, _playTime);
    }
    
    private IEnumerator MoveFromTo(Vector3 start, Vector3 dest, float totalTime)
    {
        float alpha = 0;
        
        while (alpha<1.0)
        {
            alpha += Time.deltaTime/totalTime;
            yield return transform.position = Vector3.Lerp(start, dest, alpha);
        }

        yield return _position = Util.ConvertPositionFromWorld(dest);
    }
    
    public IEnumerator Setup(Vector2Int pos)
    {
        _height = 1.5f;
        _position = pos;

        Vector3 worldPosition = Util.ConvertPositionToWorld(_position, _height);
        
        yield return (MoveFromTo(worldPosition + Vector3.down * 10, worldPosition + Vector3.up * 2, 1f));
        yield return (MoveFromTo(worldPosition + Vector3.up   * 2 , worldPosition));
    }

    public void SetNextPos(Vector2Int newPos)
    {
        _nextPosition = newPos;
    }
    
    public void Fall()
    {
        _isAlive = false;
        _capsule.SetActive(false);
    }

    public IEnumerator ApplyMovement()
    {
        _isMoving = true;
        Debug.Log("on applique un movement");
        yield return (MoveFromTo(Util.ConvertPositionToWorld(_position, _height), Util.ConvertPositionToWorld(_nextPosition, _height)));
        _isMoving = false;
    }

    public void JumpInVoid(Vector2Int pos)
    {
        _position = pos;
        Fall();
    }
    
    public IEnumerator UTurn()
    {
        _isMoving = true;
        Debug.Log("on applique un u-turn");
        yield return (CoUTurn(_position, _nextPosition));
        _isMoving = false;
    }

    public void SetUTurn(bool uTurnMax)
    {
        _willUTurn = true;
        _uTurnMax = uTurnMax;
    }

    public static Player Load(string name, bool isAlive, Vector2Int position)
    {
        Debug.Log(position + " => " + Util.ConvertPositionToWorld(position, 1.5f));
        Player p = Instantiate(GameManager.Instance.GetPlayerPrefab(), GameManager.Instance.PlayersGO.transform, true);
        p.transform.position = Util.ConvertPositionToWorld(position, 1.5f);
        Debug.Log(p.gameObject.transform.position);

        p.name = name;
        p._isAlive = isAlive;
        p._position = position;
        
        return p;
    }
}
