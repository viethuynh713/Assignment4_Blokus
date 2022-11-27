
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public int size { get; set; }
    [SerializeField] private Tilemap _boardMap;
    [SerializeField] private TileBase _ground;
    //[SerializeField] private TileBase _wall;
    [SerializeField] private TileBase _redBrick;
    [SerializeField] private TileBase _blueBrick;
    [SerializeField] private TileBase _greenBrick;
    [SerializeField] private TileBase _yellowBrick;

    List<List<BrickColor>> boardLogic;
    public List<List<BrickColor>> BoardLogic { get => boardLogic; }

    public void initMap()
    {
        size = FindObjectOfType<GameManager>().GetBoardSize();
        for (int i = -size / 2; i < size / 2; i++)
        {
            for (int j = -size / 2; j < size / 2; j++)
            {
                _boardMap.SetTile(new Vector3Int(i, j), _ground);
            }
        }
        boardLogic = new List<List<BrickColor>>();
        for (int i = 0; i < size + 2; i++)
        {
            List<BrickColor> row = new List<BrickColor>();
            for (int j = 0; j < size + 2; j++)
            {
                row.Add(BrickColor.NONE);
            }
            boardLogic.Add(row);
        }
        boardLogic[0][0] = BrickColor.RED;
        boardLogic[0][size + 1] = BrickColor.BLUE;
        boardLogic[size + 1][0] = BrickColor.GREEN;
        boardLogic[size + 1][size + 1] = BrickColor.YELLOW;
    }

    public bool placeBrick(GameObject brick, Vector2 worldPosition)
    {
        if (FindObjectOfType<GameManager>().isMyTurn())
        {
            if (isBrickOnBoard(brick, worldPosition))
            {
                Vector2Int gridPos = worldToGridPositon(worldPosition, false);
                foreach (Transform tile in brick.transform)
                {
                    Sprite tileSprite = tile.gameObject.GetComponent<SpriteRenderer>().sprite;
                    Vector3Int tilePosOnGrid = (Vector3Int)gridPos + (Vector3Int)worldToGridPositon(tile.localPosition, true);
                    _boardMap.SetTile(tilePosOnGrid, getTileBase(tileSprite));
                    Vector2Int tilePosOnGridLogic = gridViewPosToGridLogicPos((Vector2Int)tilePosOnGrid);
                    boardLogic[tilePosOnGridLogic.x][tilePosOnGridLogic.y] = getColor(tileSprite);
                }
                return true;
            }
        }
        return false;
    }

    public void placeBrickByAI(GameObject brick, Vector2Int logicPos, BrickColor color)
    {
        Vector3Int brickPosOnGrid = (Vector3Int)gridLogicPosToGridViewPos(logicPos);
        foreach (Transform tile in brick.transform)
        {
            Sprite tileSprite = tile.gameObject.GetComponent<SpriteRenderer>().sprite;
            Vector2Int tilePosByBrickOnGrid = worldToGridPositon(tile.localPosition, true);
            Vector3Int tilePosOnGrid = brickPosOnGrid + (Vector3Int)tilePosByBrickOnGrid;
            _boardMap.SetTile(tilePosOnGrid, getTileBase(color));
            Vector2Int tilePosOnLogic = logicPos + tilePosByBrickOnGrid;
            boardLogic[tilePosOnLogic.x][tilePosOnLogic.y] = color;
        }
    }

    bool isBrickOnBoard(GameObject brick, Vector2 worldPosition)
    {
        Vector2Int gridPos = worldToGridPositon(worldPosition, false);
        foreach (Transform child in brick.transform)
        {
            if (!isTileOnBoard(child, gridPos))
            {
                return false;
            }
        }
        return isBrickPlacedValid(brick, gridPos);
    }

    bool isTileOnBoard(Transform tile, Vector2Int brickPosOnGrid)
    {
        Vector2Int tilePosOnGrid = brickPosOnGrid + worldToGridPositon(tile.localPosition, true);
        if (tilePosOnGrid.x >= -size / 2 && tilePosOnGrid.x < size / 2
            && tilePosOnGrid.y >= -size / 2 && tilePosOnGrid.y < size / 2)
        {
            return true;
        }
        return false;
    }

    bool isBrickPlacedValid(GameObject brick, Vector2Int brickPosOnGrid)
    {
        Vector2Int brickPosOnGridLogic = gridViewPosToGridLogicPos(brickPosOnGrid);
        BrickColor color = getColor(brick.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite);
        return isBrickPlacedValidOnLogic(brick, brickPosOnGridLogic, color);
    }

    public bool isBrickPlacedValidOnLogic(GameObject brick, Vector2Int brickPosOnGridLogic, BrickColor color)
    {
        bool isAdjacentWithCorner = false;
        foreach (Transform tile in brick.transform)
        {
            Vector2Int tilePosOnGridLogic = brickPosOnGridLogic + worldToGridPositon(tile.localPosition, true);
            int i = tilePosOnGridLogic.x;
            int j = tilePosOnGridLogic.y;
            if (i < 1 || i >= boardLogic.Count - 1 || j < 1 || j >= boardLogic[0].Count - 1)
            {
                return false;
            }
            if (boardLogic[i][j] != BrickColor.NONE)
            {
                return false;
            }
            if (boardLogic[i - 1][j] == color || boardLogic[i + 1][j] == color
                || boardLogic[i][j - 1] == color || boardLogic[i][j + 1] == color)
            {
                return false;
            }
            if (boardLogic[i - 1][j - 1] == color || boardLogic[i - 1][j + 1] == color
                || boardLogic[i + 1][j - 1] == color || boardLogic[i + 1][j + 1] == color)
            {
                isAdjacentWithCorner = true;
            }
        }
        return isAdjacentWithCorner;
    }

    public bool isTilePlacedValid(Vector2Int logicPos, BrickColor color)
    {
        int i = logicPos.x;
        int j = logicPos.y;
        if (boardLogic[i][j] != BrickColor.NONE)
        {
            return false;
        }
        if (boardLogic[i - 1][j] == color || boardLogic[i + 1][j] == color
            || boardLogic[i][j - 1] == color || boardLogic[i][j + 1] == color)
        {
            return false;
        }
        if (boardLogic[i - 1][j - 1] == color || boardLogic[i - 1][j + 1] == color
            || boardLogic[i + 1][j - 1] == color || boardLogic[i + 1][j + 1] == color)
        {
            return true;
        }
        return false;
    }

    public Vector2Int worldToGridPositon(Vector2 worldPosition, bool isTile)
    {
        Vector2 gridPosition = worldPosition;
        if (!isTile)
        {
            gridPosition -= (Vector2)transform.position;
        }
        int x = Mathf.RoundToInt(gridPosition.x / _boardMap.cellSize.x);
        int y = Mathf.RoundToInt(gridPosition.y / _boardMap.cellSize.y);
        return new Vector2Int(x, y);
    }

    public Vector2Int gridViewPosToGridLogicPos(Vector2Int gridViewPos)
    {
        int x = gridViewPos.x + size / 2 + 1;
        int y = gridViewPos.y + size / 2 + 1;
        return new Vector2Int(x, y);
    }

    public Vector2Int gridLogicPosToGridViewPos(Vector2Int gridLogicPos)
    {
        int x = gridLogicPos.x - 1 - size / 2;
        int y = gridLogicPos.y - 1 - size / 2;
        return new Vector2Int(x, y);
    }

    public BrickColor getColor(Sprite sprite)
    {
        if (sprite == FindObjectOfType<Constant>().blueBrick)
        {
            return BrickColor.BLUE;
        }
        if (sprite == FindObjectOfType<Constant>().yellowBrick)
        {
            return BrickColor.YELLOW;
        }
        if (sprite == FindObjectOfType<Constant>().greenBrick)
        {
            return BrickColor.GREEN;
        }
        if (sprite == FindObjectOfType<Constant>().redBrick)
        {
            return BrickColor.RED;
        }
        return BrickColor.NONE;
    }

    public TileBase getTileBase(Sprite sprite)
    {
        if (sprite == FindObjectOfType<Constant>().blueBrick)
        {
            return _blueBrick;
        }
        if (sprite == FindObjectOfType<Constant>().yellowBrick)
        {
            return _yellowBrick;
        }
        if (sprite == FindObjectOfType<Constant>().greenBrick)
        {
            return _greenBrick;
        }
        if (sprite == FindObjectOfType<Constant>().redBrick)
        {
            return _redBrick;
        }
        return _ground;
    }

    public TileBase getTileBase(BrickColor color)
    {
        if (color == BrickColor.BLUE)
        {
            return _blueBrick;
        }
        if (color == BrickColor.YELLOW)
        {
            return _yellowBrick;
        }
        if (color == BrickColor.GREEN)
        {
            return _greenBrick;
        }
        if (color == BrickColor.RED)
        {
            return _redBrick;
        }
        return _ground;
    }
}
