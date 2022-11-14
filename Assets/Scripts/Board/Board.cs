using System.Collections;
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
    }
}
