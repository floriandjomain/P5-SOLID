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
    public Random Rnd = new Random();

    
    [SerializeField] private ArenaManager _arenaManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private MovementManager _movementManager;
    [SerializeField] private CameraManager _cameraManager;
    
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameState _gameState;
    
    [SerializeField] private int _turn;
    [SerializeField] private bool _preparedToLoad = false;

    [SerializeField] private CounterCoroutine _textCoroutine;

    [SerializeField] private string[] _playerCheatCode;
    [SerializeField] private bool _useCheat;

    [SerializeField] private GameEventWithString _event;

    private void Awake()
    {
	    if (_instance != null && _instance != this) Destroy(gameObject);
	        _instance = this;

        if (!_useCheat) return;
        
        PlayersSetCheat();
        
        for (int i = 0; i < _playerCheatCode.Length; i++)
        {
            _playerCheatCode[i] = _playerCheatCode[i].Replace(" ", "_");
        }
    }

    private void Update()
    {
        if(!(_gameState.GetState() == GameState.State.GameListening || _gameState.GetState() == GameState.State.OnPlay)) return;
        
        if (_useCheat && Input.GetKeyDown(KeyCode.W))
        {
            List<string> alivePlayersName = _playerManager.GetAllAlivePlayersName();

            foreach (string cheater in Cheaters)
                if (!alivePlayersName.Contains(cheater)) return;
            
            Dictionary<string, Player> players = _playerManager.GetPlayers();

            foreach (string pName in alivePlayersName)
            {
                if (Cheaters.Contains(pName)) players[pName].Fall();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            SaveSystem.Instance.SaveData();
        //else if (Input.GetKeyDown(KeyCode.L))
        //    StartCoroutine(LoadData());
    }
    
    public Arena GetArena() => _arenaManager.Arena;

    public int GetState() => _gameState.ToInt();

    public Dictionary<string, Player> GetPlayers() => _playerManager.GetPlayers();

    public Tile GetTilePrefab()
    {
        return _arenaManager.GetTilePrefab();
    }

    public Player GetPlayerPrefab()
    {
        return _playerManager.PlayerPrefab;
    }

    public List<Vector3> GetAllAlivePlayersCapsulePosition() => _playerManager.GetAllAlivePlayersCapsulePosition();

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

    public IEnumerator SetUp()
    {
        Debug.Log("[GameManager] start game setup...");
        _turn = 0;
        _playerManager.SetUp(_gameSettings.PlayTime);
        _arenaManager.SetUp(_playerManager.GetPlayers().Count, _gameSettings.TileMaxLifePoints, CheckForFalls);
        _cameraManager.SetUp(_playerManager, _arenaManager.GetMapSize());
        
        _event.Raise("");
        UIManager.Instance.ShowPlayerName();
        yield return PlacePlayers();

        UIManager.Instance.HidePlayerName();

        _movementManager.SetUp(_playerManager);
        yield return null;
        //cameraManager.UpdatePosition();
        Debug.Log("[GameManager] ...game setup done");
    }
    
    private void PlayersSetCheat()
    {
        //Debug.Log("[GameManager] !!!PlayersSetCheat code activated!!!");
        foreach (string playerName in _playerCheatCode)
        {
            _playerManager.AddPlayer(playerName.Replace(" ", "_"));
            //SetMovement(playerName, Movement.None);
            //Debug.Log(playerName + " will move" + MovementManager.ToString(move));
        }
        //Debug.Log("[GameManager] !!!PlayersSetCheat code used!!!");
    }

    private void PlayersGetMoveCheat(int random = 2)
    {
        Movement move = Movement.None+random%5;

        //Debug.Log("[GameManager] <color=red>!!!PlayersMoveCheat code activated!!!</color>");
        foreach (string playerName in _playerCheatCode)
            SetMovement(playerName, move);
        //Debug.Log("[GameManager] <color=red>!!!PlayersMoveCheat code used!!!</color>");
    }

    public void ForceTurn()
    {
        if (!_useCheat) return;

        float time = Time.deltaTime*100000;
        PlayersGetMoveCheat((int)time);
        StartCoroutine(PlayTurn());
    }

    private IEnumerator LoadData()
    {
        _preparedToLoad = true;

        while (_gameState.GetState() != GameState.State.OnPause)
            yield return null;
        

        SaveSystem.Instance.LoadData();
        yield return null;

        _preparedToLoad = false;
    }

    private IEnumerator PlacePlayers()
    {
        Dictionary<string, Player> players = _playerManager.GetPlayers();
        List<string> playerNames = new List<string>(players.Keys);

        List<Vector2Int> walkableTiles = _arenaManager.GetWalkableTilesPositions();
        
        foreach (string player in playerNames)
        {
            Vector2Int tile = walkableTiles[Rnd.Next(walkableTiles.Count)];
            _event.Raise(player);

            yield return players[player].Setup(tile);
            walkableTiles.Remove(tile);
            //Debug.Log($"[GameManager] placing player {player} at position {players[player].GetPos()}");
        }
    }

    private void CheckForFalls()
    {
        _playerManager.Falls(_arenaManager.GetTiles());
    }

    private void CompileMovements()
    {
        Dictionary<string, Player> players = _playerManager.GetPlayers();
        Dictionary<string, Movement> movements = _movementManager.GetMovements();
        
        Arena arena = _arenaManager.Arena;
        
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
        if(!player.IsAlive) return;
        
        Vector2Int pos = player.Position + MovementData.GetVector(movement);

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
        _movementManager.SetMovement(player, move);
    }

    private bool GameIsOn() => _playerManager.GetCurrentAlivePlayerNumber() > 1;

    public IEnumerator StartGame()
    {
        _gameState.AlivePlayers = new List<string>(_playerManager.GetPlayers().Keys);
        Debug.Log($"[GameManager] Début de partie {_playerManager.GetCurrentAlivePlayerNumber()}");
        
        while (GameIsOn())
        {
            Coroutine c = StartCoroutine(_textCoroutine.ExecuteCoroutine());
            TwitchClientSender.SendMessageAsync($"On vous écoute pendant {_gameSettings.CommandInputTime}sec");
            _gameState.SetState(GameState.State.GameListening);
            yield return new WaitForSeconds(_gameSettings.CommandInputTime);
            yield return c;
            
            TwitchClientSender.SendMessageAsync("Vos gueules vous parlez trop");
            _gameState.SetState(GameState.State.OnPlay);
            
            yield return StartCoroutine(PlayTurn());
            
            List<string> alivePlayersName = new List<string>(_playerManager.GetAllAlivePlayersName());
            //Debug.Log($"[GameManager] liste des joueurs {liste.Count}");
            if(alivePlayersName.Count>0) _gameState.AlivePlayers = alivePlayersName;
            //Debug.Log($"[GameManager] liste des joueurs dans gamestate {gameState.AlivePlayers.Count}");

            if (!_preparedToLoad) continue;
            
            _gameState.SetState(GameState.State.OnPause);
                
            while (_preparedToLoad)
                yield return null;
        }

        string msg = $"Partie Finie en {_turn} tours";
        //Debug.Log($"[GameManager] {msg}");
        
        foreach (string playerName in _gameState.AlivePlayers)
        {
            //msg += $"\n- {playerName}";
            //Debug.Log($"[GameManager] \n- {playerName}");
        }

        TwitchClientSender.SendMessageAsync(msg);

        UIManager.Instance.DisplayEndText(_gameState.AlivePlayers);
    }

    private IEnumerator PlayTurn()
    {
        _turn++;
        Debug.Log("PlayTurn");
        _arenaManager.Turn();
        List<Vector2Int> tilesToDamage = _playerManager.GetAllAlivePlayersPosition();
        
        Debug.Log("Compile Movements");
        if(_useCheat) PlayersGetMoveCheat();
        CompileMovements();
        yield return _playerManager.Turn();
        yield return null;

        Debug.Log("Damage Tiles");
        _arenaManager.DamageTiles(tilesToDamage);
        
        Debug.Log("Reset Movements");
        _movementManager.ResetMovements();
    }

    public void ArenaTurn()
    {
        _arenaManager.Turn();
    }

    public void StartPlayerCoroutine(IEnumerator enumerator) => StartCoroutine(enumerator);

    public void ResetPlayers()
    {
        _playerManager.RemoveAllPlayers();
    }

    public void Load(Dictionary<string, Player> playersObj, Arena arena, GameState.State state, int turn)
    {
        _playerManager.Load(playersObj);
        _arenaManager.Load(arena);
        _gameState.SetState(state);
        _turn = turn;
        _cameraManager.SetUp(_playerManager, _arenaManager.GetMapSize());
        //cameraManager.UpdatePosition();
    }
}
