using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Camera")]
public class CameraManager : ScriptableObject
{
    private Camera _camera;
    public void SetUp(PlayerManager playerManager)
    {
        playerManager.taarniendo += UpdatePosition;
        _camera = Camera.main;
        _camera.transform.rotation = Quaternion.LookRotation(Vector3.down);
    }

    public void UpdatePosition()
    {
        Vector3 newPos = Vector3.zero;

        List<Vector3> alivePlayersCapsulePosition = GameManager.Instance.GetAllAlivePlayersCapsulePosition();
        int alivePlayersCount = alivePlayersCapsulePosition.Count;

        if (alivePlayersCount < 2) return;
        
        foreach (Vector3 position in alivePlayersCapsulePosition)
            newPos += position;
            
        newPos = newPos / alivePlayersCount + (Vector3.up * 2.5f) * alivePlayersCount;
        //Debug.Log("camPos=" + _camera.transform.position + ", newPos=" + newPos);
        _camera.transform.position = newPos;
    }
}
