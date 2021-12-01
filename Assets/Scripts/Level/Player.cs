using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int nextPosition;
    [SerializeField] private bool isAlive;
        
    // Start is called before the first frame update
    private void Start()
    {
        isAlive = true;
    }

    public void SetPos(Vector2Int pos)
    {
        SetNextPos(pos);
        ApplyMovement();
    }

    public Vector2Int GetPos() => position;

    public void SetNextPos(Vector2Int newPos) => nextPosition = newPos;
    
    public Vector2Int GetNextPos() => nextPosition;

    public void Fall() => isAlive = false;
        
    public bool IsAlive() => isAlive;

    public void ApplyMovement()
    {
        position = nextPosition;
        transform.position = new Vector3(position.x * 2.5f, 1.5f , position.y * 2.5f);
    }
}
