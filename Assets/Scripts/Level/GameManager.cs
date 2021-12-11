using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    public GameObject ArenaGO;
    public GameObject PlayersGO;

    [SerializeField] private ArenaManager arenaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private GameState gameState;

    [SerializeField] private CounterCoroutine _textCoroutine;
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

    private void PlayersSetCheat()
    {
        //Debug.Log("[GameManager] !!!PlayersSetCheat code activated!!!");
        foreach (string playerName in PlayerCheatCode)
        {
            playerManager.AddPlayer(playerName.Replace(" ", "_"));
            //SetMovement(playerName, Movement.None);
            //Debug.Log(playerName + " will move" + MovementManager.ToString(move));
        }
        //Debug.Log("[GameManager] !!!PlayersSetCheat code used!!!");
    }

    private void PlayersGetMoveCheat(int random = 2)
    {
        Movement move = Movement.None+random%5;

        //Debug.Log("[GameManager] <color=red>!!!PlayersMoveCheat code activated!!!</color>");
        foreach (string playerName in PlayerCheatCode)
            SetMovement(playerName, move);
        //Debug.Log("[GameManager] <color=red>!!!PlayersMoveCheat code used!!!</color>");
    }

    public IEnumerator SetUp()
    {
        Debug.Log("[GameManager] start game setup...");
        playerManager.SetUp(gameSettings.PlayTime);
        arenaManager.SetUp(playerManager.GetPlayers().Count, gameSettings.TileMaxLifePoints, CheckForFalls);
        PlacePlayers();
        movementManager.SetUp(playerManager);
        yield return null;
        cameraManager.SetUp(playerManager);
        cameraManager.UpdatePosition();
        Debug.Log("[GameManager] ...game setup done");
    }

    private void PlacePlayers()
    {
        Dictionary<string, Player> players = playerManager.GetPlayers();
        List<string> playerNames = new List<string>(players.Keys);

        List<Vector2Int> walkableTiles = arenaManager.GetWalkableTilesPositions();
        
        foreach (string player in playerNames)
        {
            Vector2Int tile = walkableTiles[rnd.Next(walkableTiles.Count)];
            players[player].Setup(tile);
            walkableTiles.Remove(tile);
            //Debug.Log($"[GameManager] placing player {player} at position {players[player].GetPos()}");
        }
    }

    private void CheckForFalls()
    {
        playerManager.Falls(arenaManager.GetTiles());
    }

    private IEnumerator PlayTurn()
    {
        Debug.Log("PlayTurn");
        arenaManager.Turn();
        List<Vector2Int> tilesToDamage = playerManager.GetAllAlivePlayersPosition();
        
        Debug.Log("Compile Movements");
        if(UseCheat) PlayersGetMoveCheat();
        CompileMovements();
        yield return playerManager.Turn();
        yield return null;

        Debug.Log("Damage Tiles");
        arenaManager.DamageTiles(tilesToDamage);
        
        Debug.Log("Reset Movements");
        movementManager.ResetMovements();
    }

    private void CompileMovements()
    {
        Dictionary<string, Player> players = playerManager.GetPlayers();
        Dictionary<string, Movement> movements = movementManager.GetMovements();
        
        Arena arena = arenaManager.GetArena();
        
        List<string> playerNames = new List<string>(players.Keys);
        
        Debug.Log($"playerName.Count={playerNames.Count}");
        
        foreach (string playerName in playerNames)
        {
            Player p = players[playerName];
            Movement m = movements[playerName];
            Debug.Log($"{playerName} move {m}");
            CompileMovement(p, m, arena);
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
        Debug.Log($"[GameManager] {player} -> {move}");
        movementManager.SetMovement(player, move);
    }

    private bool GameIsOn() => playerManager.GetCurrentAlivePlayerNumber() > 1;

    public IEnumerator StartGame()
    {
        Debug.Log("[GameManager] Début de partie");
        
        gameState.AlivePlayers = new List<string>(playerManager.GetPlayers().Keys);
        
        while (GameIsOn())
        {
            StartCoroutine(_textCoroutine.ExecuteCoroutine());
            TwitchClientSender.SendMessageAsync($"On vous écoute pendant {gameSettings.CommandInputTime}sec");
            gameState.SetState(GameState.State.GameListening);
            yield return new WaitForSeconds(gameSettings.CommandInputTime);
            
            TwitchClientSender.SendMessageAsync("Vos gueules vous parlez trop");
            gameState.SetState(GameState.State.OnPlay);
            
            yield return StartCoroutine(PlayTurn());
            
            List<string> liste = new List<string>(playerManager.GetAllAlivePlayersName());
            //Debug.Log($"[GameManager] liste des joueurs {liste.Count}");
            if(liste.Count>0) gameState.AlivePlayers = liste;
            //Debug.Log($"[GameManager] liste des joueurs dans gamestate {gameState.AlivePlayers.Count}");
        }

        string msg = "Partie Finie";
        //Debug.Log($"[GameManager] {msg}");
        
        foreach (string playerName in gameState.AlivePlayers)
        {
            //msg += $"\n- {playerName}";
            //Debug.Log($"[GameManager] \n- {playerName}");
        }

        TwitchClientSender.SendMessageAsync(msg);

        UIManager.Instance.DisplayEndText(gameState.AlivePlayers);
    }

    public void ForceTurn()
    {
        if (!UseCheat) return;

        float time = Time.deltaTime*100000;
        PlayersGetMoveCheat((int)time);
        PlayTurn();
    }

    public void ArenaTurn()
    {
        arenaManager.Turn();
    }

    public void StartPlayerCoroutine(IEnumerator enumerator)
    {
        Debug.Log("[GameManager] EUGNEUGNEUH STARTPLAYERROUTINE");
        StartCoroutine(enumerator);
    }
}
