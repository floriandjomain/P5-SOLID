using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [SerializeField] private ArenaManager arenaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private GameState gameState;
    private Random rnd = new Random();

    [SerializeField] private string[] PlayerCheatCode;
    [SerializeField] private bool UseCheat;

    private void Awake()
    {
	    if (_instance != null && _instance != this) Destroy(gameObject);
	        _instance = this;

        if (UseCheat)
        {
            PlayersSetCheat();
            for (int i = 0; i < PlayerCheatCode.Length; i++)
            {
                PlayerCheatCode[i] = PlayerCheatCode[i].Replace(" ", "_");
            }
        }
        //StartCoroutine(SetUp());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && UseCheat)
        {
            float time = Time.deltaTime*100000;
            PlayersGetMoveCheat((int)time);
            PlayTurn();
        }

        if (Input.GetKeyDown(KeyCode.E)) arenaManager.Turn();
    }

    private void PlayersSetCheat()
    {
        //Debug.Log("!!!PlayersSetCheat code activated!!!");
        foreach (string playerName in PlayerCheatCode)
        {
            playerManager.AddPlayer(playerName.Replace(" ", "_"));
            //SetMovement(playerName, Movement.None);
            //Debug.Log(playerName + " will move" + MovementManager.ToString(move));
        }
        //Debug.Log("!!!PlayersSetCheat code used!!!");
    }

    private void PlayersGetMoveCheat(int random)
    {
        Movement move = Movement.None+random%5;

        //Debug.Log("<color=red>!!!PlayersMoveCheat code activated!!!</color>");
        foreach (string playerName in PlayerCheatCode)
            SetMovement(playerName, move+(rnd.Next(5)));
        //Debug.Log("<color=red>!!!PlayersMoveCheat code used!!!</color>");
    }

    public IEnumerator SetUp()
    {
        //Debug.Log("start game setup...");
        playerManager.SetUp();
        arenaManager.SetUp(playerManager.GetPlayers().Count, gameSettings.TileMaxLifePoints, CheckForFalls);
        PlacePlayers();
        movementManager.SetUp(playerManager);
        yield return null;
        cameraManager.SetUp(playerManager);
        //_cameraManager.UpdatePosition();
        //Debug.Log("...game setup done");
    }

    private void PlacePlayers()
    {
        Dictionary<string, Player> players = playerManager.GetPlayers();
        List<string> playerNames = new List<string>(players.Keys);

        List<Vector2Int> walkableTiles = arenaManager.GetWalkableTilesPositions();
        
        foreach (string player in playerNames)
        {
            Vector2Int tile = walkableTiles[rnd.Next(walkableTiles.Count)];
            players[player].SetPos(tile);
            walkableTiles.Remove(tile);
            //Debug.Log("placing player " + player + " at position " + players[player].GetPos());
        }
    }

    private void CheckForFalls()
    {
        playerManager.Falls(arenaManager.GetTiles());
    }

    private void PlayTurn()
    {
        arenaManager.DamageTiles(playerManager.GetAllAlivePlayersPosition());
        CompileMovements();
        playerManager.Turn(arenaManager.GetTiles());
    }

    private void CompileMovements()
    {
        
        Dictionary<string, Player> players = playerManager.GetPlayers();
        Dictionary<string, Movement> movements = movementManager.GetMovements();
        
        Arena arena = arenaManager.GetArena();
        
        List<string> playerNames = new List<string>(players.Keys);
        
        foreach (string p in playerNames)
        {
            Player _p = players[p];
            Movement m = movements[p];
            CompileMovement(_p, m, arena);
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
        if (gameState.GetState() == GameState.State.GameListening)
        {
            Debug.Log("[GameManager] " + player + " " + move);
            movementManager.SetMovement(player, move);
        }
    }

    private bool GameIsOn() => playerManager.GetCurrentAlivePlayerNumber() > 1;

    public IEnumerator StartGame()
    {
        Debug.Log("Début de partie");
        
        gameState.AlivePlayers = new List<string>(playerManager.GetPlayers().Keys);
        
        while (GameIsOn())
        {
            TwitchClientSender.SendMessage("On vous écoute pendant 10sec");
            gameState.SetState(GameState.State.GameListening);
            yield return new WaitForSeconds(gameSettings.CommandInputTime);
            TwitchClientSender.SendMessage("Vos gueules vous parlez trop");
            gameState.SetState(GameState.State.OnPlay);
            yield return new WaitForSeconds(gameSettings.PlayTime);
            PlayTurn();
            List<string> liste = new List<string>(playerManager.GetPlayers().Keys);
            Debug.Log("liste des joueurs " + liste.Count);
            gameState.AlivePlayers = liste;
            Debug.Log("liste des joueurs dans gamestate " + gameState.AlivePlayers.Count);
        }

        string msg = "Partie Finie";
        
        foreach (string playerName in gameState.AlivePlayers)
        {
            msg += "\n- " + playerName;
        }
        
        TwitchClientSender.SendMessage(msg);
    }
}
