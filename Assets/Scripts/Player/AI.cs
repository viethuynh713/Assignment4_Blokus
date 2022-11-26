using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public Move(Move move)
        {
            brick = move.brick;
            pos = move.pos;
            point = move.point;
        }
        public bool isNull()
        {
            return brick == null;
        }
        public void checkPoint(GameObject brick, Vector2Int pos, int point, bool isMax)
        {
            if (isMax)
            {
                if (brick == null || point >= this.point)
                {
                    this.brick = brick;
                    this.pos = pos;
                    this.point = point;
                }
            }
            else
            {
                if (brick == null || point <= this.point)
                {
                    this.brick = brick;
                    this.pos = pos;
                    this.point = point;
                }
            }
        }
        public void checkPoint(Move move, bool isMax)
        {
            checkPoint(move.brick, move.pos, move.point, isMax);
        }
    }

    List<List<BrickColor>> board;
    List<GameObject> ListBricks;
    BrickColor color;
    Board boardComponent;
    List<BrickColor> colorList;
    public int depth = 1;
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
        colorList = FindObjectOfType<GameManager>().getPlayerColorList();
        Move move = calcMove(depth, color, board, new Move(), depth);
        if (move.isNull())
        {
            GetComponent<Player>().pass();
        }
        else
        {
            boardComponent.placeBrickByAI(move.brick, move.pos, color);
            GetComponent<Player>().removeBrick(move.brick);
        }
    }

    Move calcMove(int depth, BrickColor color, List<List<BrickColor>> board, Move mainMove, int h)
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
                                List<List<BrickColor>> boardClone = placeBrickToTest(brick, brickPos, board);
                                if (depth > 1)
                                {
                                    if (depth == h)
                                    {
                                        mainMove.brick = brick;
                                        mainMove.pos = brickPos;
                                    }
                                    Move subMove = calcMove(depth - 1, getNextColor(color), boardClone, mainMove, h);
                                    move.checkPoint(subMove, color == this.color);
                                }
                                else
                                {
                                    move.checkPoint(brick, brickPos, calcPoint(boardClone), color == this.color);
                                    if (depth == h)
                                    {
                                        mainMove.brick = move.brick;
                                        mainMove.pos = move.pos;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return mainMove;
    }

    List<List<BrickColor>> placeBrickToTest(GameObject brick, Vector2Int brickPos, List<List<BrickColor>> board)
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
        return boardClone;
    }

    int calcPoint(List<List<BrickColor>> boardClone)
    {
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

    BrickColor getNextColor(BrickColor color)
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            if (colorList[i] == color)
            {
                if (i == colorList.Count - 1)
                {
                    return colorList[0];
                }
                return colorList[i + 1];
            }
        }
        return BrickColor.NONE;
    }
}
