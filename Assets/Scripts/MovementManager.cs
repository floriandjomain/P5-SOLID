using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private static MovementManager _instance;

    public static MovementManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        _instance = this;

    }

    public void MoveUp(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 1, obj.transform.position.z);
    }

    public void MoveRight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x + 1, obj.transform.position.y, obj.transform.position.z);
    }

    public void MoveDown(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 1, obj.transform.position.z);
    }

    public void MoveLeft(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x - 1, obj.transform.position.y, obj.transform.position.z);
    }
}
