using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]private List<Vector2> _shape;

    private bool isSelected;
    public List<Vector2> BrickShape
    {
        get => _shape;
    }
    private void Start()
    {
        isSelected = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (isMouseOn())
            {
                Rotation();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isMouseOn())
            {
                isSelected = !isSelected;
                viewSelected();
            }
            else
            {
                if (isSelected)
                {
                    if (FindObjectOfType<Board>().placeBrick(gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
                        Destroy(gameObject);
                    }
                    else
                    {
                        isSelected = false;
                        viewSelected();
                    }
                }
            }
        }
    }

    public void Rotation()
    {
        foreach (Transform child in transform)
        {
            child.localPosition = new Vector2(child.localPosition.y, -child.localPosition.x);
        }
    }

    public void setPositionByGrid(float gridSize)
    {
        foreach (Transform child in transform)
        {
            child.localPosition *= gridSize;
        }
    }

    private bool isMouseOn()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float differences = 0.5f;
        return mousePos.x - differences < transform.position.x && mousePos.x + differences > transform.position.x
                && mousePos.y - differences < transform.position.y && mousePos.y + differences > transform.position.y;
    }

    void viewSelected()
    {
        foreach (Transform child in transform)
        {
            if (isSelected)
            {
                child.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                child.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
