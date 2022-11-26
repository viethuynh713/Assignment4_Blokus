using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int turn;
    public List<GameObject> BlokusPlayers = new List<GameObject>();
    public GameState State = GameState.INIT;

    [SerializeField] private Grid _mainGrid;
    [SerializeField] private int nPlayer = 0;
    [SerializeField] private string playerID;
    public BrickColor playerColor; // get from server
    [SerializeField] private GameObject playerSample;
    [SerializeField] private List<GameObject> ListBricks;
    [SerializeField] private List<Vector2> brickPosOnFieldList;
    [SerializeField] private List<Sprite> tileSpriteList;

    [SerializeField] private Board m_boardGame;
    [SerializeField] private PhotonView view;
    List<BrickColor> colorList;

    [SerializeField] private RoomNetwork _roomNetwork;
    

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        colorList = new List<BrickColor>();
        // BrickColor[] colorListSample = new BrickColor[4] { BrickColor.BLUE, BrickColor.YELLOW, BrickColor.GREEN, BrickColor.RED };
        // for (int i = 0; i < nPlayer; i++)
        // {
        //     colorList.Add(colorListSample[i]);
        // }
        // foreach (BrickColor iColor in colorList)
        // {
        //     Debug.Log(iColor);
        //     GameObject player = Instantiate(playerSample);
        //     player.transform.SetParent(transform, false);
        //     player.GetComponent<BUPlayer>().init(iColor, playerColor, !(playerColor == iColor));
        //     player.GetComponent<BUPlayer>().initBrickOnField(ListBricks, brickPosOnFieldList, tileSpriteList[(int)iColor], _mainGrid.cellSize.x);
        //     BlokusPlayers.Add(player);
        // }
        // FindObjectOfType<GameUI>().initPlayerPanelList(nPlayer);
        // turn = 0;
        // BlokusPlayers[turn].GetComponent<BUPlayer>().Play();
    }
    public void StartGameUI(int i)
    {
        view.RPC("StartGame",RpcTarget.All,i);
    }
    [PunRPC]
    public void StartGame(int numOfPlayer)
    {
        // Init board
        nPlayer = numOfPlayer;
        m_boardGame.size = GetBoardSize(nPlayer);
        m_boardGame.initMap();

        // Init Player
        foreach (var item in _roomNetwork._listPlayerUIEnable)
        {
            colorList.Add((BrickColor)item.Value.index);
            
            GameObject player = Instantiate(playerSample);
            player.transform.SetParent(transform, false);
            player.GetComponent<BUPlayer>().init((BrickColor)item.Value.index, (BrickColor)item.Value.index, item.Value.IsBot);
            player.GetComponent<BUPlayer>().initBrickOnField(ListBricks, brickPosOnFieldList, tileSpriteList[(int)item.Value.index], _mainGrid.cellSize.x);
            BlokusPlayers.Add(player);

        }


        // Init UI
        GameUI.instance.initPlayerPanelList(nPlayer);
        turn = 0;
        BlokusPlayers[turn].GetComponent<BUPlayer>().Play();
        
    }
    public int GetBoardSize(int i)
    {
        switch (i)
        {
            case 4:
                return 20;            
            case 3:
                return 16;
            case 2:
                return 12;
        }
        return 0;
    }
    [PunRPC]
    public void SwitchPlayer()
    {
        if (isEndGame())
        {
            endGame();
        }
        else
        {
            turn++;
            if (turn == nPlayer)
            {
                turn = 0;
            }
            Debug.Log("SwitchPlayer: " + turn);
            FindObjectOfType<GameUI>().switchPlayerUI(turn);
            BlokusPlayers[turn].GetComponent<BUPlayer>().Play();
        }
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

    public GameObject getMyPlayer()
    {
        foreach (GameObject player in BlokusPlayers)
        {
            if (player.GetComponent<BUPlayer>().Color == playerColor)
            {
                return player;
            }
        }
        return null;
    }

    public List<BrickColor> getPlayerColorList()
    {
        return colorList;
    }

    public void passTurn()
    {
        if (isMyTurn())
        {
            getMyPlayer().GetComponent<BUPlayer>().pass();
        }
    }

    public bool isEndGame()
    {
        foreach (GameObject blokusPlayer in BlokusPlayers)
        {
            if (!blokusPlayer.GetComponent<BUPlayer>().IsPassed)
            {
                return false;
            }
        }
        return true;
    }

    public void endGame()
    {
        // get point
        List<int> order = new List<int>();
        List<int> pointList = new List<int>();
        for (int i = 0; i < BlokusPlayers.Count; i++)
        {
            order.Add(i);
            pointList.Add(BlokusPlayers[i].GetComponent<BUPlayer>().calcPoint());
        }
        // sort point
        bool hasResult = false;
        do
        {
            hasResult = true;
            for (int i = 0; i < order.Count - 1; i++)
            {
                if (pointList[order[i]] < pointList[order[i + 1]])
                {
                    hasResult = false;
                    int t = order[i];
                    order[i] = order[i + 1];
                    order[i + 1] = t;
                }
            }
        }
        while (!hasResult);
        // get result
        List<int> result = new List<int>();
        foreach (int idx in order)
        {
            result.Add(0);
        }
        int lowestRank = 1;
        for (int i = 0; i < order.Count; i++)
        {
            if (i > 0)
            {
                if (pointList[order[i]] != pointList[order[i - 1]])
                {
                    lowestRank++;
                }
            }
            result[order[i]] = lowestRank;
        }
        FindObjectOfType<GameUI>().printResult(result);
    }
}
