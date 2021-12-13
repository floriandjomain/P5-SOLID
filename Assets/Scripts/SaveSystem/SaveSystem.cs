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
    
    public void LoadData()
    {
        _stopwatch.Restart();
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} LoadData : Start");
        LoadGame(ReadData());
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} LoadData : End");
    }

    private void LoadGame(Task<SaveData> readData)
    {
        SaveData data = readData.Result;

        foreach (PlayerStruct player in data.Players)
        {
            GameManager.Instance.SetPlayer(player.Name, player.Position, player.IsAlive);
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
            case "ApocalypseArena":
                arena = ScriptableObject.CreateInstance<ApocalypseArena>();
                break;
        }
        
        foreach (TileStruct tile in data.Arena.Tiles)
        {
            tile.Name;
            tile.Position;
            tile.LifePoints;
            tile.Timer;
        }
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
        Tile[,] tilesObj = arenaObj.GetTiles();
        
        TileStruct[,] tiles = new TileStruct[tilesObj.GetLength(0),tilesObj.GetLength(1)];

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Tile tileObj = tilesObj[i, j];
                
                tiles[i, j] = new TileStruct()
                {
                    LifePoints = (int)tileObj.GetLife(),
                    Name =  tileObj.name,
                    Position = new Vector2Int(i,j)
                };
            }
            
            yield return null;
        }

        ArenaStruct arena = new ArenaStruct()
        {
            ArenaType = arenaObj.GetType().Name,
            Tiles = tiles,
            Scale = Vector3.one
        };

        List<PlayerStruct> players = new List<PlayerStruct>();

        foreach (string playerName in playersObj.Keys)
        {
            PlayerStruct player = new PlayerStruct()
            {
                Name = playerName,
                IsAlive = playersObj[playerName].IsAlive(),
                Position = playersObj[playerName].GetPos()
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
        
        Debug.Log($"[SaveSystem] {_stopwatch.ElapsedMilliseconds} WriteData : Start");
    }
    
    #endregion
}