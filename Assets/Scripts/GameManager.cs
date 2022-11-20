using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const int SIZE = 20;

    public readonly List<List<int>> BlokusMap = new List<List<int>>();
    public List<GameObject> BlokusPlayers = new List<GameObject>();
    public GameState State = GameState.INIT;

    [SerializeField] private Grid _mainGrid;
    [SerializeField] private int nPlayer = 4;
    [SerializeField] private string playerID;
    [SerializeField] private BrickColor playerColor; // get from server
    [SerializeField] private GameObject playerSample;
    [SerializeField] private List<GameObject> ListBricks;
    [SerializeField] private List<Vector2> brickPosOnFieldList;
    [SerializeField] private List<Sprite> tileSpriteList;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        List<BrickColor> colorList;
        colorList = new List<BrickColor>();
        switch (nPlayer)
        {
            case 2:
                colorList.Add(BrickColor.BLUE);
                colorList.Add(BrickColor.GREEN);
                break;
            case 4:
                colorList.Add(BrickColor.BLUE);
                colorList.Add(BrickColor.YELLOW);
                colorList.Add(BrickColor.GREEN);
                colorList.Add(BrickColor.RED);
                break;
            default:
                break;
        }
        foreach (BrickColor iColor in colorList)
        {
            GameObject player = Instantiate(playerSample);
            player.transform.SetParent(transform, false);
            player.GetComponent<Player>().Color = iColor;
            if (playerColor == iColor)
            {
                player.GetComponent<Player>().IsMyPlayer = true;
            }
            else
            {
                player.GetComponent<Player>().IsMyPlayer = false;
            }
            player.GetComponent<Player>().initBrickOnField(ListBricks, brickPosOnFieldList, tileSpriteList[(int)iColor], _mainGrid.cellSize.x);
            BlokusPlayers.Add(player);
        }
    }
    private void Update()
    {
        
    }
    [PunRPC]
    public void SwitchPlayer()
    {

    }
}
