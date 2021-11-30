using UnityEngine;

[RequireComponent(typeof(Material))]
public class Tile : MonoBehaviour
{
    [SerializeField] private Material HealthyTile;
    [SerializeField] private Material MidLifeTile;
    [SerializeField] private Material LastLifeTile;
    [SerializeField] private int StartLifePoints;
    [SerializeField] private int CurrentLifePoints;

    private void Start()
    {
        CurrentLifePoints = StartLifePoints;
        GetComponent<Material>().color = HealthyTile.color;
    }

    public void Damage()
    {
        CurrentLifePoints--;

        if (CurrentLifePoints > StartLifePoints / 2) return;
        
        if (CurrentLifePoints == 1)
            GetComponent<Material>().color = LastLifeTile.color;
        else if (CurrentLifePoints == StartLifePoints / 2)
            GetComponent<Material>().color = MidLifeTile.color;
        else if (CurrentLifePoints == 0)
            gameObject.SetActive(false);
    }
}
