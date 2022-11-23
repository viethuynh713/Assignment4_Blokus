using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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
    public bool IsAI { get => isMyPlayer; set { isMyPlayer = value; } }

    public Player(string iD, string name, BrickColor color)
    {
        ID = iD;
        Name = name;
        this.color = color;
    }

    void Start()
    {
        ListBricks = new List<GameObject>();
    }

    public void Play()
    {
        IsMyTurn = true;
        StartCoroutine(delay(5));
    }

    [PunRPC]
    public void PutBrick()
    {

    }

    public void initBrickOnField(List<GameObject> brickList, List<Vector2> brickPosOnFieldList, Sprite sprite, float gridSize)
    {
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
    }

    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        IsMyTurn = false;
        FindObjectOfType<GameManager>().SwitchPlayer();
    }
}
