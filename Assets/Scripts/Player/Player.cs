using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string ID;
    public string Name;
    public BrickColor color;
    private bool _isMyTurn = false;
    public bool IsMyTurn { get => _isMyTurn; set { _isMyTurn = value; } }

    public List<Brick> ListBricks;

    public Player(string iD, string name, BrickColor color)
    {
        ID = iD;
        Name = name;
        this.color = color;
        ListBricks = new List<Brick>
        {
            
        };
    }
    [PunRPC]
    public void PutBrick()
    {

    }

}
