using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material healthyTile;
    [SerializeField] private Material midLifeTile;
    [SerializeField] private Material lastLifeTile;
    [SerializeField] private int startLifePoints;
    [SerializeField] private int currentLifePoints;
    [SerializeField] private GameObject cube;
    private void Awake()
    {
        currentLifePoints = startLifePoints;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material.color = healthyTile.color;
        cube.transform.localScale = new Vector3(2.5f, 0.5f, 2.5f);
    }

    public void Damage()
    {
        currentLifePoints--;

        if (currentLifePoints > startLifePoints / 2) return;
        
        if (currentLifePoints == 1)
            cube.GetComponent<Renderer>().material.color = lastLifeTile.color;
        else if (currentLifePoints == startLifePoints / 2)
            cube.GetComponent<Renderer>().material.color = midLifeTile.color;
        else if (currentLifePoints == 0)
            gameObject.SetActive(false);
    }
}
