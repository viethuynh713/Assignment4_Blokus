using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public Sprite blueBrick;
    public Sprite greenBrick;
    public Sprite yellowBrick;
    public Sprite redBrick;
    public int negativeInf = -99999999;
    public Vector2Int nullVector = new Vector2Int(-99999999, -99999999);

    public bool isNullVector(Vector2Int v)
    {
        if (v == nullVector)
        {
            return true;
        }
        return false;
    }

    public bool isNullNumber(int n)
    {
        if (n == negativeInf)
        {
            return true;
        }
        return false;
    }
}
