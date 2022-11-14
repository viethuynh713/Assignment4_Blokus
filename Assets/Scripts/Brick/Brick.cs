using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]private List<Vector2> _shape;
    public List<Vector2> BrickShape
    {
        get => _shape;
    }
    private void Start()
    {
        
    }

    public void Rotation()
    {

    }

    public void setPositionByGrid(float gridSize)
    {
        foreach (Transform child in transform)
        {
            child.position *= gridSize;
        }
    }
}
