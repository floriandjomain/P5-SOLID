using System;
using System.Collections;
using System.Collections.Generic;
using static System.Random;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [SerializeField] private ArenaManager arenaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameSettings gameSettings;

    [SerializeField] private string[] PlayerCheatCode;
    [SerializeField] private bool UseCheat;

    private void Awake()
    {
	    if (_instance != null && _instance != this) Destroy(gameObject);
	        _instance = this;

        if(UseCheat) PlayersSetCheat();
        StartCoroutine(SetUp());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && UseCheat)
        {
            float time = Time.deltaTime*100000;
            PlayersGetMoveCheat((int)time);
            PlayTurn();
        }

        if (Input.GetKeyDown(KeyCode.E)) arenaManager.ErodeArena();
    }

    private void PlayersSetCheat()
    {
        //Debug.Log("!!!PlayersSetCheat code activated!!!");
        foreach (string playerName in PlayerCheatCode)
        {
            playerManager.AddPlayer(playerName);
            SetMovement(playerName, Movement.None);
            //Debug.Log(playerName + " will move" + MovementManager.ToString(move));
        }
        //Debug.Log("!!!PlayersSetCheat code used!!!");
    }

    private void PlayersGetMoveCheat(int random)
    {
        Movement move = Movement.None+random%5;

        System.Random rnd = new System.Random();

        //Debug.Log("<color=red>!!!PlayersMoveCheat code activated!!!</color>");
        foreach (string playerName in PlayerCheatCode)
            SetMovement(playerName, move+(rnd.Next(5)));
        //Debug.Log("<color=red>!!!PlayersMoveCheat code used!!!</color>");
    }

    public IEnumerator SetUp()
    {
        //Debug.Log("start game setup...");
        playerManager.SetUp();
        arenaManager.SetUp(playerManager.GetPlayers(), gameSettings.TileMaxLifePoints, CheckForFalls);
        movementManager.SetUp(playerManager);
        yield return null;
        cameraManager.SetUp(playerManager);
        //_cameraManager.UpdatePosition();
        //Debug.Log("...game setup done");
    }

    private void CheckForFalls()
    {
        playerManager.Falls(arenaManager.GetTiles());
    }

    public void PlayTurn()
    {
        arenaManager.DamageTiles(playerManager.GetPlayers());
        CompileMovements();
        playerManager.Turn(arenaManager.GetTiles(), movementManager.GetMovements());
    }

    private void CompileMovements()
    {
        
        Dictionary<string, Player> players = playerManager.GetPlayers();
        Dictionary<string, Movement> movements = movementManager.GetMovements();
        Arena arena = arenaManager.GetArena();
        
        List<string> playerNames = new List<string>(players.Keys);
        
        foreach (string p in playerNames)
        {
            CompileMovement(players[p], movements[p], arena);
        }
    }
    
    private void CompileMovement(Player player, Movement movement, Arena arena)
    {
        if(!player.IsAlive()) return;
        
        Vector2Int pos = player.GetPos() + MovementData.GetVector(movement);

        if (!arena.IsInArena(pos))
        {
            player.JumpInVoid(pos);
            return;
        }

        player.SetNextPos(pos);
    }

    public void SetMovement(string player, Movement move)
    {
        movementManager.SetMovement(player, move);
    }
}
