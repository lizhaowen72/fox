using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallx : MonoBehaviour
{
    public Transform camera;
    public float moveRate;
    private float startPointX, startPointY;
    public bool lockY;


    // Start is called before the first frame update
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + camera.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + camera.position.x * moveRate, startPointY + camera.position.y * moveRate);
        }

    }
}
