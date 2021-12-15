using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int nextPosition;
    [SerializeField] private GameObject capsule;
    [SerializeField] private bool isAlive;
    [SerializeField] private bool willUTurn;
    [SerializeField] private float height;
    [SerializeField] public bool IsMoving;
    public float PlayTime;
    private bool uTurnMax;

    // Start is called before the first frame update
    private void Start()
    {
        isAlive = true;
        willUTurn = false;
        transform.position = Vector3.down * 2;
    }

    private IEnumerator CoUTurn(Vector2 start, Vector2 falseEnd)
    {
        Vector3 worldStart = ConvertPositionToWorld(start);
        Vector3 worldFalseEnd = ConvertPositionToWorld(falseEnd);
        Vector3 colPosition = (start * (uTurnMax?2:1) + falseEnd ) / (uTurnMax?3:2);
        
        yield return StartCoroutine(MoveFromTo(worldStart, colPosition, PlayTime/2));
        yield return StartCoroutine(MoveFromTo(colPosition, worldStart, PlayTime/2));
    }

    private IEnumerator MoveFromTo(Vector3 start, Vector3 dest)
    {
        yield return MoveFromTo(start, dest, PlayTime);
    }
    
    private IEnumerator MoveFromTo(Vector3 start, Vector3 dest, float totalTime)
    {
        float Alpha = 0;
        
        while (Alpha<1.0)
        {
            Alpha += Time.deltaTime/totalTime;
            yield return transform.position = Vector3.Lerp(start, dest, Alpha);
        }

        yield return position = ConvertPositionFromWorld(dest);
    }

    private Vector2Int ConvertPositionFromWorld(Vector3 vec)
    {
        vec -= Vector3.up * height;
        vec /= 2.5f;
        return new Vector2Int((int)vec.x, (int)vec.z);
    }

    private Vector3 ConvertPositionToWorld(Vector2 vec)
    {
        return new Vector3(vec.x, 0f, vec.y) * 2.5f + Vector3.up * height;
    }
    
    public IEnumerator Setup(Vector2Int pos)
    {
        height = name == "flupiiipi" ? 2f:1.5f;
        position = pos;
        yield return (MoveFromTo(ConvertPositionToWorld(position) + Vector3.down * 10, ConvertPositionToWorld(position) + Vector3.up * 2, 1f));
        yield return (MoveFromTo(ConvertPositionToWorld(position) + Vector3.up * 2   , ConvertPositionToWorld(position)));
        //transform.position = new Vector3(position.x, 0f, position.y) * 2.5f + Vector3.up * height;
    }

    public Vector2Int GetPos() => position;

    public void SetNextPos(Vector2Int newPos)
    {
        nextPosition = newPos;
    }
    
    public Vector2Int GetNextPos() => nextPosition;

    public void Fall()
    {
        isAlive = false;
        capsule.SetActive(false);
        //MessageQueue.Instance.SendMessage("@" + gameObject.name + " just fell");
    }
        
    public bool IsAlive() => isAlive;

    public IEnumerator ApplyMovement()
    {
        IsMoving = true;
        Debug.Log("on applique un movement");
        yield return (MoveFromTo(ConvertPositionToWorld(position), ConvertPositionToWorld(nextPosition)));
        IsMoving = false;
    }

    public Vector3 GetCapsulePos() => capsule.transform.position;

    public void JumpInVoid(Vector2Int pos)
    {
        position = pos;
        Fall();
    }

    public bool WillUTurn() => willUTurn;
    
    public IEnumerator UTurn()
    {
        IsMoving = true;
        Debug.Log("on applique un u-turn");
        yield return (CoUTurn(position, nextPosition));
        IsMoving = false;
    }

    public void SetUTurn(bool _uTurnMax)
    {
        willUTurn = true;
        uTurnMax = _uTurnMax;
    }

    public static Player Load(string Name, bool IsAlive, Vector2Int Position)
    {
        Player p = Instantiate(GameManager.Instance.GetPlayerPrefab(), GameManager.Instance.PlayersGO.transform, true);

        p.name = Name;
        p.isAlive = IsAlive;
        p.position = Position;
        
        return p;
    }
}
