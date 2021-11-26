using System;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

namespace Level
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Tile[][] Tiles;

        [SerializeField] private List<Player> Players;
        [SerializeField] private Movement[] Movements;

        [SerializeField] private GameSettings _gameSettings;
        
        public void PlayTurn()
        {
            DamageTiles();
            CompileMovements();
            ApplyMovements();
            Falls();
            ResetMovements();
        }

        private void ApplyMovements()
        {
            bool[] conflicts = new bool[Players.Count];
            
            for(int b=0; b < conflicts.Length; b++)
                conflicts[b] = false;
            
            for (int p1 = 0; p1 < Players.Count-1; p1++)
            {
                for (int p2 = p1+1; p2 < Players.Count; p2++)
                {
                    if (Players[p1].GetNextPos() == Players[p2].GetNextPos())
                    {
                        conflicts[p1] = true;
                        conflicts[p2] = true;
                    }
                }
            }
            
            for(int player=0; player < conflicts.Length; player++)
            {
                if (!conflicts[player])
                {
                    Players[player].ApplyMovement();
                    //triggerevent deplacement pour l'animation
                }
                else
                {
                    //triggerevent allerretour pour l'animation
                }
            }
        }

        private void DamageTiles()
        {
            Vector2Int pos;

            foreach (var p in Players)
            {
                if (p.IsAlive())
                {
                    pos = p.GetPos();
                    Tiles[pos.x][pos.y].Damage();
                }
            }
        }

        private void Falls()
        {
            Vector2Int pos;

            foreach (var p in Players)
            {
                if (p.IsAlive())
                {
                    pos = p.GetPos();
                    
                    if (Tiles[pos.x][pos.y])
                        p.Fall();
                }
            }
        }

        private void CompileMovements()
        {
            for (int p = 0; p < Players.Count; p++)
            {
                CompileMovement(Players[p], Movements[p]);
            }
        }

        private void CompileMovement(Player player, Movement movement)
        {
            Vector2Int pos = player.GetPos() + MovementManager.GetVector(movement);

            if (pos.x < 0 || pos.y < 0 || pos.x >= Tiles.Length || pos.y >= Tiles[0].Length) return;

            if (!Tiles[pos.x][pos.y].isActiveAndEnabled) return;

            player.SetNextPos(pos);
        }

        public void SetMovement(int player, Movement move)
        {
            Movements[player] = move;
        }

        private void ResetMovements()
        {
            for (int i=0; i<Movements.Length; i++)
            {
                Movements[i] = Movement.NONE;
            }
        }
    }
}
