using UnityEngine;

namespace Level
{
    public class Player : MonoBehaviour
    {
        private Vector2Int position;
        private Vector2Int nextPosition;
        private bool isAlive;
        
        // Start is called before the first frame update
        void Start()
        {
            isAlive = true;
        }

        public Vector2Int GetPos() => position;

        public void SetNextPos(Vector2Int newPos) => nextPosition = newPos;
        public Vector2Int GetNextPos() => nextPosition;

        public void Fall() => isAlive = false;
        
        public bool IsAlive() => isAlive;

        public void ApplyMovement() => position = nextPosition;
    }
}
