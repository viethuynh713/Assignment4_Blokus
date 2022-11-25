using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI : MonoBehaviour
{
    public class Move
    {
        public GameObject brick;
        public Vector2Int pos;
        public int point;
        public Move()
        {
            brick = null;
            pos = FindObjectOfType<Constant>().nullVector;
            point = FindObjectOfType<Constant>().negativeInf;
        }
        public bool isNull()
        {
            return brick == null;
        }
        public void checkPoint(GameObject brick, Vector2Int pos, int point)
        {
            if (point > this.point)
            {
                this.brick = brick;
                this.pos = pos;
                this.point = point;
            }
        }
    }

    List<List<BrickColor>> board;
    List<GameObject> ListBricks;
    BrickColor color;
    Board boardComponent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init(BrickColor color)
    {
        boardComponent = FindObjectOfType<Board>();
        this.color = color;
    }

    public void play()
    {
        board = FindObjectOfType<Board>().BoardLogic;
        ListBricks = GetComponent<Player>().ListBricks;
        Move move = calcMove();
        if (!move.isNull())
        {
            boardComponent.placeBrickByAI(move.brick, move.pos, color);
            GetComponent<Player>().removeBrick(move.brick);
        }
    }

    Move calcMove()
    {
        Move move = new Move();
        for (int i = 1; i < board.Count - 1; i++)
        {
            for (int j = 1; j < board[0].Count - 1; j++)
            {
                if (boardComponent.isTilePlacedValid(new Vector2Int(i, j), color))
                {
                    foreach (GameObject brick in ListBricks)
                    {
                        foreach (Transform tile in brick.transform)
                        {
                            Vector2Int brickPos = new Vector2Int(i, j) - boardComponent.worldToGridPositon(tile.localPosition, true);
                            if (boardComponent.isBrickPlacedValidOnLogic(brick, brickPos, color))
                            {
                                move.checkPoint(brick, brickPos, calcPoint(brick, brickPos));
                            }
                        }
                    }
                }
            }
        }
        return move;
    }

    int calcPoint(GameObject brick, Vector2Int brickPos)
    {
        List<List<BrickColor>> boardClone = new List<List<BrickColor>>();
        foreach (List<BrickColor> row in board)
        {
            boardClone.Add(new List<BrickColor>(row));
        }
        foreach (Transform tile in brick.transform)
        {
            Vector2Int tilePosOnGrid = brickPos + boardComponent.worldToGridPositon(tile.localPosition, true);
            boardClone[tilePosOnGrid.x][tilePosOnGrid.y] = color;
        }
        int point = 0;
        for (int i = 1; i < boardClone.Count - 1; i++)
        {
            for (int j = 1; j < boardClone[0].Count - 1; j++)
            {
                foreach (BrickColor brickColor in FindObjectOfType<GameManager>().getPlayerColorList())
                {
                    if (boardComponent.isTilePlacedValid(new Vector2Int(i, j), brickColor))
                    {
                        if (brickColor == color)
                        {
                            point++;
                        }
                        else
                        {
                            point--;
                        }
                    }
                }
            }
        }
        return point;
    }

    void placeBrick(GameObject brick, Vector2Int pos)
    {

    }
}
