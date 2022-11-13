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
    public  void Rotation()
    {

    }
}
