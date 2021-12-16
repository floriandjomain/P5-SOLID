using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Camera")]
public class CameraManager : ScriptableObject
{
    private Camera _camera;
    
    public void SetUp(PlayerManager playerManager, int mapSize)
    {
        //playerManager.taarniendo += UpdatePosition;
        _camera = Camera.main;
        _camera.transform.rotation = Quaternion.LookRotation(Vector3.down);
        _camera.transform.position = new Vector3((mapSize-1) * 1.25f, mapSize * 2.5f, (mapSize-1) * 1.25f);
    }

    public void UpdatePosition()
    {
        Vector3 newPos = Vector3.zero;

        List<Vector3> alivePlayersCapsulePosition = GameManager.Instance.GetAllAlivePlayersCapsulePosition();
        int alivePlayersCount = alivePlayersCapsulePosition.Count;

        if (alivePlayersCount < 2) return;

        float maxDist = 0f;

        foreach (Vector3 p1 in alivePlayersCapsulePosition)
        {
            newPos += p1;
            
            foreach (Vector3 p2 in alivePlayersCapsulePosition)
            {
                float distance = Vector3.Distance(p1, p2);

                if (distance > maxDist) maxDist = distance;
            }
        }

        newPos = newPos / alivePlayersCount * 1f + (Vector3.up * maxDist);
        //Debug.Log("camPos=" + _camera.transform.position + ", newPos=" + newPos);
        _camera.transform.position = newPos;
    }
}
