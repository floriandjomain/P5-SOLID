using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    public GameObject ArenaGO;
    public GameObject PlayersGO;
    public List<string> Cheaters;

    [SerializeField] private ArenaManager arenaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private GameState gameState;
    [SerializeField] private int turn;
    [SerializeField] private bool preparedToLoad = false;

    [SerializeField] private CounterCoroutine _textCoroutine;
    private Random rnd = new Random();

    [SerializeField] private string[] PlayerCheatCode;
    [SerializeField] private bool UseCheat;

    private void Awake()
    {
	    if (_instance != null && _instance != this) Destroy(gameObject);
	        _instance = this;

        if (!UseCheat) return;
        
        PlayersSetCheat();
        
        for (int i = 0; i < PlayerCheatCode.Length; i++)
        {
            PlayerCheatCode[i] = PlayerCheatCode[i].Replace(" ", "_");
        }
    }

    private void Update()
    {
        if(!(gameState.GetState() == GameState.State.GameListening || gameState.GetState() == GameState.State.OnPlay)) return;
        
        if (UseCheat && Input.GetKeyDown(KeyCode.W))
        {
            List<string> alivePlayersName = playerManager.GetAllAlivePlayersName();

            foreach (string cheater in Cheaters)
                if (!alivePlayersName.Contains(cheater)) return;
            
            Dictionary<string, Player> players = playerManager.GetPlayers();

            foreach (string pName in alivePlayersName)
            {
                if (Cheaters.Contains(pName)) players[pName].Fall();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            SaveSystem.Instance.SaveData();
        else if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(LoadData());
    }

    public void ClearPlayerAndTiles()
    {
        foreach (Transform child in ArenaGO.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in PlayersGO.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator LoadData()
    {
        preparedToLoad = true;

        while (gameState.GetState() != GameState.State.OnPause)
            yield return null;
        

        SaveSystem.Instance.LoadData();
        yield return null;

        preparedToLoad = false;
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
        turn = 0;
        cameraManager.SetUp(playerManager);
        playerManager.SetUp(gameSettings.PlayTime);
        arenaManager.SetUp(playerManager.GetPlayers().Count, gameSettings.TileMaxLifePoints, CheckForFalls);
        yield return PlacePlayers();
        movementManager.SetUp(playerManager);
        yield return null;
        cameraManager.UpdatePosition();
        Debug.Log("[GameManager] ...game setup done");
    }

    private IEnumerator PlacePlayers()
    {
        Dictionary<string, Player> players = playerManager.GetPlayers();
        List<string> playerNames = new List<string>(players.Keys);

        List<Vector2Int> walkableTiles = arenaManager.GetWalkableTilesPositions();
        
        foreach (string player in playerNames)
        {
            Vector2Int tile = walkableTiles[rnd.Next(walkableTiles.Count)];
            yield return players[player].Setup(tile);
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
        turn++;
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
        gameState.AlivePlayers = new List<string>(playerManager.GetPlayers().Keys);
        Debug.Log($"[GameManager] Début de partie {playerManager.GetCurrentAlivePlayerNumber()}");
        
        while (GameIsOn())
        {
            Coroutine c = StartCoroutine(_textCoroutine.ExecuteCoroutine());
            TwitchClientSender.SendMessageAsync($"On vous écoute pendant {gameSettings.CommandInputTime}sec");
            gameState.SetState(GameState.State.GameListening);
            yield return new WaitForSeconds(gameSettings.CommandInputTime);
            yield return c;
            
            TwitchClientSender.SendMessageAsync("Vos gueules vous parlez trop");
            gameState.SetState(GameState.State.OnPlay);
            
            yield return StartCoroutine(PlayTurn());
            
            List<string> alivePlayersName = new List<string>(playerManager.GetAllAlivePlayersName());
            //Debug.Log($"[GameManager] liste des joueurs {liste.Count}");
            if(alivePlayersName.Count>0) gameState.AlivePlayers = alivePlayersName;
            //Debug.Log($"[GameManager] liste des joueurs dans gamestate {gameState.AlivePlayers.Count}");

            if (!preparedToLoad) continue;
            
            gameState.SetState(GameState.State.OnPause);
                
            while (preparedToLoad)
                yield return null;
        }

        string msg = $"Partie Finie en {turn} tours";
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
        StartCoroutine(PlayTurn());
    }

    public void ArenaTurn()
    {
        arenaManager.Turn();
    }

    public Arena GetArena() => arenaManager.GetArena();

    public void StartPlayerCoroutine(IEnumerator enumerator) => StartCoroutine(enumerator);

    public int GetState() => gameState.ToInt();

    public Dictionary<string, Player> GetPlayers() => playerManager.GetPlayers();

    public void ResetPlayers()
    {
        playerManager.RemoveAllPlayers();
    }

    public Tile GetTilePrefab()
    {
        return arenaManager.GetTilePrefab();
    }

    public Player GetPlayerPrefab()
    {
        return playerManager.GetPlayerPrefab();
    }

    public void Load(Dictionary<string, Player> playersObj, Arena arena, GameState.State state, int _turn)
    {
        playerManager.Load(playersObj);
        arenaManager.Load(arena);
        gameState.SetState(state);
        turn = _turn;
        cameraManager.SetUp(playerManager);
        cameraManager.UpdatePosition();
    }

    public List<Vector3> GetAllAlivePlayersCapsulePosition() => playerManager.GetAllAlivePlayersCapsulePosition();
}
