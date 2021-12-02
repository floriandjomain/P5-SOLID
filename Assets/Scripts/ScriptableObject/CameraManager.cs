using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Camera")]
public class CameraManager : ScriptableObject
{
    private PlayerManager _playerManager;
    private Camera _camera;
    public void SetUp(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _playerManager.taarniendo += UpdatePosition;
        _camera = Camera.main;
        _camera.transform.rotation = Quaternion.LookRotation(Vector3.down);
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 newPos = Vector3.zero;

        List<Vector3> alivePlayersCapsulePosition = _playerManager.GetAllAlivePlayersCapsulePosition();
        int alivePlayersCount = alivePlayersCapsulePosition.Count;

        if (alivePlayersCount <= 0)
        {
            Debug.Log("POURQUOI");
            return;
        }
        
        foreach (Vector3 position in alivePlayersCapsulePosition)
            newPos+=position;
            
        newPos = newPos / alivePlayersCount + (Vector3.up * 2.5f) * alivePlayersCount;
        Debug.Log("camPos=" + _camera.transform.position + ", newPos=" + newPos);
        _camera.transform.position = newPos;
    }
}
