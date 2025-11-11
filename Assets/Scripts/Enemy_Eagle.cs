using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    //private Collider2D coll;
    //private Animator anim;

    public float Speed;
    public Transform toppoint, bottompoint;
    private float TopY, BottomY;
    private bool isUp = true;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        //anim = GetComponent<Animator>();
        transform.DetachChildren();
        TopY = toppoint.position.y;
        BottomY = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > TopY)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < BottomY)
            {
                isUp = true;
            }
        }

    }
}
