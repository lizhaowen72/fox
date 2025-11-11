using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D disColl;
    [Space]
    public float speed;
    public float jumpForce;
    public AudioSource jumpAudioSource;
    public AudioSource hurtAudioSource;
    public AudioSource cherryAudioSource;
    public AudioSource runningAudioSource;

    [Space]
    public LayerMask ground;
    public int Cherry;
    public Text cherryNum;
    private bool isHurt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isHurt)
        {
            Movement();
        }
        SwitchAnim();
    }

    void Movement()
    {
        float horizontalMove;
        horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        // 角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            runningAudioSource.Play();
            anim.SetFloat("running", Mathf.Abs(facedirection));
        }
        // 角色转向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        // 角色跳跃
       if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            jumpAudioSource.Play();
            anim.SetBool("jumping", true);
        }
        Crouch();
    }

    // 切换动画
    void SwitchAnim()
    {
        anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                // rb的速度在y轴上小于0时，表示开始降落，转换动画
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            // 如果碰撞到地面，转换动画
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    // 收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry += 1;
            cherryAudioSource.Play();
            cherryNum.text = Cherry.ToString();
        }
    }

    // 消灭敌人
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Debug.Log("left");
                rb.velocity = new Vector2(-10, rb.velocity.y);
                hurtAudioSource.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                Debug.Log("right");
                rb.velocity = new Vector2(10, rb.velocity.y);
                hurtAudioSource.Play();
                isHurt = true;
            }
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            anim.SetBool("crouching", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            anim.SetBool("crouching", false);
        }
    }
}
