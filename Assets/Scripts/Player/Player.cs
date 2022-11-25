using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string ID;

    public string Name;

    private BrickColor color;
    public BrickColor Color { get => color; set { color = value; } }

    private bool _isMyTurn = false;
    public bool IsMyTurn { get => _isMyTurn; set { _isMyTurn = value; } }

    public List<GameObject> ListBricks;

    private bool isMyPlayer;
    public bool IsMyPlayer { get => isMyPlayer; set { isMyPlayer = value; } }

    private bool isAI;
    public bool IsAI { get => isAI; set { isAI = value; } }

    public Player(string iD, string name, BrickColor color)
    {
        ID = iD;
        Name = name;
        this.color = color;
    }

    void Start()
    {
        
    }

    public void init(BrickColor color, BrickColor myColor, bool isAI)
    {
        this.color = color;
        if (myColor == color)
        {
            isMyPlayer = true;
        }
        else
        {
            isMyPlayer = false;
        }
        this.isAI = isAI;
        if (this.isAI)
        {
            this.AddComponent<AI>();
            GetComponent<AI>().init(color);
        }
    }

    public void Play()
    {
        IsMyTurn = true;
        if (isAI)
        {
            GetComponent<AI>().play();
            switchToNextTurn();
        }
    }

    [PunRPC]
    public void PutBrick()
    {

    }

    public void initBrickOnField(List<GameObject> brickList, List<Vector2> brickPosOnFieldList, Sprite sprite, float gridSize)
    {
        ListBricks = new List<GameObject>();
        for (int i = 0; i < brickList.Count; i++)
        {
            GameObject brick = Instantiate(brickList[i], brickPosOnFieldList[i], Quaternion.identity);
            brick.transform.SetParent(transform, false);
            brick.GetComponent<Brick>().setPositionByGrid(gridSize);
            foreach (Transform child in brick.transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            if (!isMyPlayer)
            {
                brick.SetActive(false);
            }
            ListBricks.Add(brick);
        }
        Debug.Log("ListBricks: " + ListBricks.Count + ", " + color);
    }

    public void switchToNextTurn()
    {
        StartCoroutine(WaitForSwitchToNextTurn());
    }
    
    IEnumerator WaitForSwitchToNextTurn()
    {
        yield return new WaitForSeconds(1);
        IsMyTurn = false;
        FindObjectOfType<GameManager>().SwitchPlayer();
    }

    public void removeBrick(GameObject brick)
    {
        if (brick != null)
        {
            ListBricks.Remove(brick);
            //brick.GetComponent<Brick>().removeSelf();
        }
    }
}
