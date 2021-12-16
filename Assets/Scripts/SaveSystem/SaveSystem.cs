using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SaveSystem : MonoBehaviour
{
    #region Singleton

    private static SaveSystem _instance;

    public static SaveSystem Instance => _instance;

    private void Awake()
    {
        if (_instance!=null)
            Destroy(_instance);

        _instance = this;
    }

    #endregion
    
    [SerializeField] private string savePath;
    [SerializeField] private string saveFileName;
    [SerializeField] private string saveFileExtension;

    private Stopwatch _stopwatch = new Stopwatch();

    #region Loading
    
    public async void LoadData()
    {
        _stopwatch.Restart();
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} LoadData : Start");
        SaveData savedData = await ReadData();
        LoadGame(savedData);
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} LoadData : End");
    }

    private void LoadGame(SaveData readData)
    {
        SaveData data = readData;
        Debug.Log(JsonUtility.ToJson(data));

        // Empty GameObject for Players and Tiles
        GameManager.Instance.ClearPlayerAndTiles();

        Dictionary<string, Player> playersObj = new Dictionary<string, Player>();
        foreach (PlayerStruct player in data.Players)
        {
            playersObj.Add(player.Name, Player.Load(player.Name, player.IsAlive, player.Position));
        }

        Tile[,] tileMap = new Tile[data.Arena.SizeX, data.Arena.SizeY];
        foreach (TileStruct t in data.Arena.Tiles)
        {
            tileMap[t.Position.x, t.Position.y] = Tile.Load(t.Name, t.StartLifePoints, t.CurrentLifePoints, t.Timer);
        }

        Arena arena;
        switch (data.Arena.ArenaType)
        {
            case "Arena":
                arena = ScriptableObject.CreateInstance<Arena>();
                break;
            case "CircleArena":
                arena = ScriptableObject.CreateInstance<CircleArena>();
                break;
            default:
                arena = ScriptableObject.CreateInstance<ApocalypseArena>();
                break;
        }

        arena.Load(tileMap);
        GameManager.Instance.Load(playersObj, arena, GameState.GetStateFromInt(data.State), data.Turn);
    }

    private async Task<SaveData> ReadData()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, savePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(directoryPath, saveFileName + saveFileExtension);

        byte[] bytes;
        
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            bytes = new byte[fileStream.Length];
            await fileStream.ReadAsync(bytes, 0, (int)fileStream.Length);
        }
        
        return JsonUtility.FromJson<SaveData>
        (
            Encoding.UTF8.GetString(bytes)
        );
    }

    #endregion

    #region Save

    public void SaveData()
    {
        _stopwatch.Restart();
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} SaveData : Start");
        StartCoroutine(GetData(async data => await WriteData(data)));
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} SaveData : End");
    }
    
    private IEnumerator GetData(Action<SaveData> callback)
    {
        Dictionary<string, Player> playersObj = GameManager.Instance.GetPlayers();
        Arena arenaObj = GameManager.Instance.GetArena();
        Tile[,] tilesObj = arenaObj.Tiles;
        
        List<TileStruct> tiles = new List<TileStruct>();
        for (int i = 0; i < tilesObj.GetLength(0); i++)
        {
            for (int j = 0; j < tilesObj.GetLength(1); j++)
            {
                Tile tileObj = tilesObj[i, j];

                tiles.Add(new TileStruct()
                {
                    StartLifePoints = tileObj.StartLifePoints,
                    CurrentLifePoints = tileObj.CurrentLifePoints,
                    Name = tileObj.name,
                    Timer = tileObj.Timer,
                    Position = new Vector2Int(i, j)
                });
            }

            yield return null;
        }

        ArenaStruct arena = new ArenaStruct()
        {
            ArenaType = arenaObj.GetType().Name,
            Tiles = tiles,
            SizeX = tilesObj.GetLength(0),
            SizeY = tilesObj.GetLength(1)
        };

        List<PlayerStruct> players = new List<PlayerStruct>();
        foreach (string playerName in playersObj.Keys)
        {
            PlayerStruct player = new PlayerStruct()
            {
                Name = playerName,
                IsAlive = playersObj[playerName].IsAlive,
                Position = playersObj[playerName].Position
            };
            
            players.Add(player);
        }
        
        SaveData data = new SaveData()
        {
            State = GameManager.Instance.GetState(),
            Arena = arena,
            Players = players
        };
        
        callback.Invoke(data);
    }
    
    private async Task WriteData(SaveData data)
    {
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} WriteData : Start");
        string directoryPath = Path.Combine(Application.persistentDataPath, savePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(directoryPath, saveFileName + saveFileExtension);

        byte[] bytes = await Task.Run(() => Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
        
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} WriteData : End");
    }
    
    #endregion
}