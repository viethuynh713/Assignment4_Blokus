using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] private Tilemap _boardMap;
    [SerializeField] private TileBase _ground;
    [SerializeField] private TileBase _wall;
    [SerializeField] private TileBase _redBrick;
    [SerializeField] private TileBase _blueBrick;
    [SerializeField] private TileBase _greenBrick;
    [SerializeField] private TileBase _yellowBrick;
    [SerializeField] private Grid _mainGrid;

    public List<int> BlokusMap;

    private int _size;
    public void initMap()
    {
        BlokusMap = new List<int>();
        _size = GameManager.Instance.BoardSize;
        for (int i = -_size / 2; i < _size / 2; i++)
        {
            for (int j = -_size / 2; j < _size / 2; j++)
            {
                _boardMap.SetTile(new Vector3Int(i, j), _ground);
            }
        }
    }

     private TileBase GetTileOfPlayer(Player player) {
        switch (player.Color) {
            case BrickColor.BLUE:
                return _blueBrick;
            case BrickColor.YELLOW:
                return _yellowBrick;
            case BrickColor.RED:
                return _redBrick;
            case BrickColor.GREEN:
                return _greenBrick;
            default:
                return null;
        }
    }
    public int PlaceBrick(List<Vector2> pos, BrickColor color,Vector2 mousePos)
    {

        return 0;
    }
    public bool ValidateBrick(List<Vector2> pos)
    {
        return true;
    }
}
