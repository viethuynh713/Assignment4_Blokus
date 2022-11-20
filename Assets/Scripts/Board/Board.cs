using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
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

    // Start is called before the first frame update
    void Start()
    {
        size = 20;
        initMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initMap()
    {
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
        if (isBrickOnBoard(brick, worldPosition))
        {
            Vector2Int gridPos = worldToGridPositon(worldPosition, false);
            foreach (Transform child in brick.transform)
            {
                Vector3Int tilePosOnGrid = (Vector3Int)gridPos + (Vector3Int)worldToGridPositon(child.localPosition, true);
                _boardMap.SetTile(tilePosOnGrid, _greenBrick);
                Vector2Int tilePosOnGridLogic = gridViewPosToGridLogicPos((Vector2Int)tilePosOnGrid);
                boardLogic[tilePosOnGridLogic.x][tilePosOnGridLogic.y] = BrickColor.GREEN;
            }
            return true;
        }
        return false;
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
        //return true;
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
        bool isAdjacentWithCorner = false;
        foreach (Transform tile in brick.transform)
        {
            BrickColor color = getColor(tile.GetComponent<SpriteRenderer>().sprite);
            Vector2Int tilePosOnGrid = brickPosOnGrid + worldToGridPositon(tile.localPosition, true);
            Vector2Int tilePosOnGridLogic = gridViewPosToGridLogicPos(tilePosOnGrid);
            int i = tilePosOnGridLogic.x;
            int j = tilePosOnGridLogic.y;
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

    Vector2Int worldToGridPositon(Vector2 worldPosition, bool hasParent)
    {
        Vector2 gridPosition = worldPosition;
        if (!hasParent)
        {
            gridPosition -= (Vector2)transform.position;
        }
        int x = Mathf.RoundToInt(gridPosition.x / _boardMap.cellSize.x);
        int y = Mathf.RoundToInt(gridPosition.y / _boardMap.cellSize.y);
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

    public Vector2Int gridViewPosToGridLogicPos(Vector2Int gridViewPos)
    {
        int x = gridViewPos.x + size / 2 + 1;
        int y = gridViewPos.y + size / 2 + 1;
        return new Vector2Int(x, y);
    }
}
