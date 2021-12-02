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
        cube.GetComponent<Renderer>().material.color = healthyTile.color;
        cube.transform.localScale = new Vector3(2f, 0.5f, 2f);
    }

    public void Damage()
    {
        currentLifePoints--;

        OnDamage();
    }

    private void OnDamage()
    {
        if (currentLifePoints > startLifePoints / 2) return;
        
        if (currentLifePoints == 1)
            cube.GetComponent<Renderer>().material.color = lastLifeTile.color;
        if (currentLifePoints == startLifePoints / 2)
            cube.GetComponent<Renderer>().material.color = midLifeTile.color;
        else if (currentLifePoints == 0)
            gameObject.SetActive(false);
    }

    public void SetStartLife(int startLife)
    {
        startLifePoints   = startLife;
        currentLifePoints = startLife;
    }

    public bool IsBroken() => currentLifePoints == 0;

    public void Break()
    {
        currentLifePoints = 0;
        OnDamage();
    }
}
