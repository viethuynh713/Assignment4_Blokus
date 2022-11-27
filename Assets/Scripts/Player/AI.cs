using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using static AI;

public class AI : MonoBehaviour
{
    public class Move
    {
        public GameObject brick;
        public Vector2Int pos;
        public int point;
        public Move(bool isMaximizingPlayer)
        {
            brick = null;
            pos = FindObjectOfType<Constant>().nullVector;
            if (isMaximizingPlayer)
            {
                point = FindObjectOfType<Constant>().negativeInf;
            }
            else
            {
                point = FindObjectOfType<Constant>().positiveInf;
            }
        }
        public Move(GameObject brick, Vector2Int pos, bool isMaximizingPlayer)
        {
            this.brick = brick;
            this.pos = pos;
            if (isMaximizingPlayer)
            {
                point = FindObjectOfType<Constant>().negativeInf;
            }
            else
            {
                point = FindObjectOfType<Constant>().positiveInf;
            }
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
        public void getOptimize(Move move, bool isMaximizingPlayer)
        {
            if (isMaximizingPlayer)
            {
                if (brick == null || move.point >= point)
                {
                    brick = move.brick;
                    pos = move.pos;
                    point = move.point;
                }
            }
            else
            {
                if (brick == null || move.point <= point)
                {
                    brick = move.brick;
                    pos = move.pos;
                    point = move.point;
                }
            }
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
        ListBricks = GetComponent<BUPlayer>().ListBricks;
        colorList = FindObjectOfType<GameManager>().getPlayerColorList();
        Move move = alphaBeta(new List<Move>(), depth, new Move(true), new Move(false), color, board);
        if (move.isNull())
        {
            GetComponent<BUPlayer>().pass();
        }
        else
        {
            boardComponent.placeBrickByAI(move.brick, move.pos, color);
            GetComponent<BUPlayer>().removeBrick(move.brick);
        }
    }
    Move alphaBeta(List<Move> moveList, int depth, Move alpha, Move beta, BrickColor color, List<List<BrickColor>> boardClone)
    {
        if (depth == 0)
        {
            Move res = new Move(moveList[0]);
            res.point = calcPoint(boardClone);
            return res;
        }

        bool isMaximizingPlayer = this.color == color;
        Move value = new Move(isMaximizingPlayer);
        for (int i = 1; i < boardClone.Count - 1; i++)
        {
            for (int j = 1; j < boardClone[0].Count - 1; j++)
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
                                List<List<BrickColor>> subBoardClone = placeBrickToTest(brick, brickPos, boardClone);
                                Move move = new Move(brick, brickPos, isMaximizingPlayer);
                                List<Move> subMoveList = new List<Move>(moveList);
                                subMoveList.Add(move);
                                value.getOptimize(alphaBeta(subMoveList, depth - 1, alpha, beta, getNextColor(color), subBoardClone), isMaximizingPlayer);
                                if (isMaximizingPlayer)
                                {
                                    if (value.point >= beta.point)
                                    {
                                        break;
                                    }
                                    alpha.getOptimize(value, isMaximizingPlayer);
                                }
                                else
                                {
                                    if (value.point <= alpha.point)
                                    {
                                        break;
                                    }
                                    beta.getOptimize(value, isMaximizingPlayer);
                                }
                            }
                        }
                    }
                }
            }
        }
        return value;
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
