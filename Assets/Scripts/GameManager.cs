using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const int SIZE = 20;
    private int turn;
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

    List<BrickColor> colorList;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        colorList = new List<BrickColor>();
        BrickColor[] colorListSample = new BrickColor[4] { BrickColor.BLUE, BrickColor.YELLOW, BrickColor.GREEN, BrickColor.RED };
        for (int i = 0; i < nPlayer; i++)
        {
            colorList.Add(colorListSample[i]);
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
        turn = 0;
        FindObjectOfType<GameUI>().initPlayerPanelList(nPlayer);
        BlokusPlayers[turn].GetComponent<Player>().Play();
    }
    private void Update()
    {
        
    }
    [PunRPC]
    public void SwitchPlayer()
    {
        turn++;
        if (turn == nPlayer)
        {
            turn = 0;
        }
        BlokusPlayers[turn].GetComponent<Player>().Play();
        FindObjectOfType<GameUI>().switchPlayerUI(turn);
    }

    public bool isMyTurn()
    {
        // ???
        if (colorList[turn] == playerColor)
        {
            return true;
        }
        return false;
    }
}
