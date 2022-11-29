using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlurBrick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        blur();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void blur()
    {
        //foreach (Transform t in transform)
        //{
        //    t.GetComponent<SpriteRenderer>().color.WithAlpha(100);
        //}
    }
}
