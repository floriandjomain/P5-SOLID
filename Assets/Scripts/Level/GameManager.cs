using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private string[] playerCheatCode;
    [SerializeField] private bool UseCheat;

    private void Awake()
    {
        PlayersSetCheat();
        StartCoroutine(SetUp());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayersGetMoveCheat();
            PlayTurn();
        }
    }

    private void PlayersSetCheat()
    {
        //Debug.Log("!!!PlayersSetCheat code activated!!!");
        Movement move = Movement.None;

        foreach (string playerName in playerCheatCode)
        {
            _playerManager.AddPlayer(playerName);
            SetMovement(playerName, move);
            //Debug.Log(playerName + " will move" + MovementManager.ToString(move));
        }
        //Debug.Log("!!!PlayersSetCheat code used!!!");
    }

    private void PlayersGetMoveCheat()
    {
        //Debug.Log("<color=red>!!!PlayersMoveCheat code activated!!!</color>");
        foreach (string playerName in playerCheatCode)
            SetMovement(playerName, Movement.Right);
        //Debug.Log("<color=red>!!!PlayersMoveCheat code used!!!</color>");
    }

    public IEnumerator SetUp()
    {
        //Debug.Log("start game setup...");
        _playerManager.SetUp();
        _tileManager.SetUp(_playerManager.GetPlayers(), _gameSettings.TileMaxLifePoints);
        yield return null;
        _cameraManager.SetUp(_playerManager);
        //_cameraManager.UpdatePosition();
        //Debug.Log("...game setup done");
    }
    
    public void PlayTurn()
    {
        _tileManager.DamageTiles(_playerManager.GetPlayers());
        _playerManager.Turn(_tileManager.GetTiles());
    }

    public void SetMovement(string player, Movement move)
    {
        _playerManager.SetMovement(player, move);
    }

    public bool AddPlayer(string playerPseudo)
    {
        if (_playerManager.GetCurrentPlayerNumber() >= _gameSettings.MaxPlayerNumber) return false;
            
        _playerManager.AddPlayer(playerPseudo);
        return true;
    }

    public void RemovePlayer(string playerPseudo) => _playerManager.RemovePlayer(playerPseudo);
}
