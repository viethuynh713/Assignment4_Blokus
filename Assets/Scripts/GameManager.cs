using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const int COL = 22;
    private const int ROW = 22;

    public readonly List<List<int>> BlokusMap = new List<List<int>>();
    public List<Player> BlokusPlayers = new List<Player>();
    public GameState State = GameState.INIT;
    #region TitleMap
    [SerializeField] private Grid _mainGrid;
    [SerializeField] private Tilemap _boardMap;
    [SerializeField] private TileBase _ground;
    [SerializeField] private TileBase _wall;
    [SerializeField] private TileBase _redBrick;
    [SerializeField] private TileBase _blueBrick;
    [SerializeField] private TileBase _greenBrick;
    [SerializeField] private TileBase _yellowBrick;
    #endregion
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    [PunRPC]
    public void SwitchPlayer()
    {

    }
}
