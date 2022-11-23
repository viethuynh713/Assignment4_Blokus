using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int BoardSize = 20;
   // public List<GameObject> BlokusPlayers = new List<GameObject>();
    public Player localPlayer;
    public GameState State = GameState.INIT;
    public List<Vector2> SelectedBrick = new List<Vector2>();
    #region TitleMap
    //[SerializeField] private Grid _mainGrid;
    

    //[SerializeField] private int nPlayer = 4;
    // [SerializeField] private string playerID;
    // [SerializeField] private BrickColor playerColor; // get from server
    // [SerializeField] private GameObject playerSample;
    // [SerializeField] private List<GameObject> ListBricks;
    // [SerializeField] private List<Vector2> brickPosOnFieldList;
    // [SerializeField] private List<Sprite> tileSpriteList;

    [SerializeField] private Board _board;
    #endregion
    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayGame()
    {
        _board.initMap();
        DisplayBrick();
        GenerateStartPoint();
        State = GameState.PLAYING;
    }
    public void PauseGame(){
        State = GameState.PAUSE;
    }

    private void Update() {
        _selectBrick();
        PlaceSelectedBrick();
        RotationBrick();
    }

    private void PlaceSelectedBrick()
    {
        if(SelectedBrick.Count !=0 && _board.ValidateBrick(SelectedBrick) && Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
            _board.PlaceBrick(SelectedBrick,localPlayer.Color,mousePos2d);
        }
    }

    private void _selectBrick()
    {
        if (State == GameState.PLAYING && Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2d = new Vector2(pos.x, pos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
            // Get the value of the piece selected
            if (hit.collider != null) {
                var currentBrick = hit.collider.gameObject.GetComponent<Brick>();

                if (currentBrick != null) {
                    SelectedBrick = currentBrick.BrickShape;
                    
                }
            }
        }
    }

    private void DisplayBrick()
    {
    }

    private void GenerateStartPoint()
    {
    }

    private void Start()
    {
        
    }

    [PunRPC]
    public void SwitchPlayer()
    {

    }
    public void RotationBrick()
    {

    }
}
