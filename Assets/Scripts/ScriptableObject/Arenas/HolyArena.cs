using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Holy")]
public class HolyArena : Arena
{
    private int holyTimer = 4;
    public override void Turn()
    {
        if (holyTimer == 4)
            ComputeHoly();
        else if (holyTimer == 0)
            SetHollows();

        holyTimer = (holyTimer - 1) % 5;

        //Un timer
        //des zones safes
        //le reste en hollow
        //5 tours : un où les zones safe sont montrées
        //3 pour les atteindre
        //le dernier fait tomber tous les joueurs qui sont sur une case hollow
    }

    private void ComputeHoly()
    {
        throw new System.NotImplementedException();
    }

    private void SetHollows()
    {
        throw new System.NotImplementedException();
    }
}
