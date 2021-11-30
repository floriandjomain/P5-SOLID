using UnityEngine;

[RequireComponent(typeof(Material))]
public class Tile : MonoBehaviour
{
    [SerializeField] private Material healthyTile;
    [SerializeField] private Material midLifeTile;
    [SerializeField] private Material lastLifeTile;
    [SerializeField] private int startLifePoints;
    [SerializeField] private int currentLifePoints;

    private void Awake()
    {
        currentLifePoints = startLifePoints;
        GetComponent<Material>().color = healthyTile.color;
    }

    public void Damage()
    {
        currentLifePoints--;

        if (currentLifePoints > startLifePoints / 2) return;
        
        if (currentLifePoints == 1)
            GetComponent<Material>().color = lastLifeTile.color;
        else if (currentLifePoints == startLifePoints / 2)
            GetComponent<Material>().color = midLifeTile.color;
        else if (currentLifePoints == 0)
            gameObject.SetActive(false);
    }
}
