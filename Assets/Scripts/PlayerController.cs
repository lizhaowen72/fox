using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D disColl;
    public Transform cellingCheck;
    [Space]
    public float speed;
    public float jumpForce;
    private bool isJumping;
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
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            isJumping = true;
        }
        Crouch();
    }
    void FixedUpdate()
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
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            jumpAudioSource.Play();
            anim.SetBool("jumping", true);
            isJumping = false;
        }
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
                isJumping = false;
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

    // 碰撞触发器 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集樱桃
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry += 1;
            cherryAudioSource.Play();
            cherryNum.text = Cherry.ToString();
        }
        if (collision.tag == "Deadline")
        {
            // 将所有的音频都禁用
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
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
        // 当fox头上没有障碍物的时候，才执行下面的代码
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
