using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Holy")]
public class HolyArena : Arena
{
    private int holyZoneSize;
    private int holyTimer = 4;

    public override void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        base.MapInstantiation(playerNumber, maxTileHealth, action);

        holyZoneSize = playerNumber;
    }
    
    public override void Turn()
    {
        switch (holyTimer)
        {
            case 4:
                holyZoneSize--;
                ComputeHoly();
                break;
            case 0:
                SetHollows();
                break;
        }

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
        Debug.Log("compute holy");
    }

    private void SetHollows()
    {
        Debug.Log("set hollows");
    }
}
