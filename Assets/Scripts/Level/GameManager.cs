using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

namespace Level
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Tile[][] Tiles;

        [SerializeField] private Dictionary<string, Player> Players;
        [SerializeField] private Dictionary<string, Movement> Movements;

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
            List<string> conflictedPlayers = new List<string>();

            foreach (string p1 in Players.Keys)
            {
                foreach (string p2 in Players.Keys)
                {
                    if (p1 == p2 || Players[p1].GetNextPos() != Players[p2].GetNextPos()) continue;
                    
                    if(!conflictedPlayers.Contains(p1))
                        conflictedPlayers.Add(p1);
                    if(!conflictedPlayers.Contains(p2))
                        conflictedPlayers.Add(p2);
                }
            }

            foreach (string p in Players.Keys)
            {
                if(!conflictedPlayers.Contains(p))
                    Players[p].ApplyMovement();
            }
        }

        private void DamageTiles()
        {
            Vector2Int pos;

            foreach (string p in Players.Keys)
            {
                if (Players[p].IsAlive())
                {
                    pos = Players[p].GetPos();
                    Tiles[pos.x][pos.y].Damage();
                }
            }
        }

        private void Falls()
        {
            Vector2Int pos;

            foreach (string p in Players.Keys)
            {
                if (Players[p].IsAlive())
                {
                    pos = Players[p].GetPos();
                    
                    if (Tiles[pos.x][pos.y])
                        Players[p].Fall();
                }
            }
        }

        private void CompileMovements()
        {
            foreach (string p in Players.Keys)
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

        public void SetMovement(string player, Movement move)
        {
            Movements[player] = move;
        }

        private void ResetMovements()
        {
            foreach (string p in Movements.Keys)
            {
                Movements[p] = Movement.None;
            }
        }
    }
}
