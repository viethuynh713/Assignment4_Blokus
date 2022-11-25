using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TMPro.TMP_Text nameTxt;
    public BrickColor playerColor;
    public UnityEngine.UI.Image brickImage;
    public bool IsHost = false;

    public void SetInfos(string name, BrickColor color, bool isHost = false)
    {
        nameTxt.text = name;
        playerColor = color;
        brickImage.color = _getColor(color);
        IsHost = isHost;
    }

    private Color _getColor(BrickColor color)
    {
        switch(color)
        {
            case BrickColor.BLUE:
                return Color.blue;
                break;
            case BrickColor.RED:
                return Color.red;
                break;
            case BrickColor.GREEN:
                return Color.green;
                break;
            case BrickColor.YELLOW:
                return Color.yellow;
                break;
        }
        return Color.black;
    }
}
