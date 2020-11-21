using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private HealthBar healthBar;

    private bool isGrounded;
    private bool isDead = false;
    private bool damaged = false;
    private bool facingRight = true;
    private bool jump = false;
    private float speed;
    private float nextAttackTime;
    private float damageTimer;
    private int health;
    private int lives;

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight = -9;
    public float maxHeight = 4;
    public float attackRate = 2;
    public float damageTime = 0.5f;
    public int maxHealth = 60;
    public int maxLives = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        speed = maxSpeed;
        health = maxHealth;
        lives = maxLives;
        isGrounded = true;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetText("x" + maxLives.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Dead", isDead);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump = true;
            isGrounded = false;
        }
        /*
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("Attack");
            //nextAttackTime = Time.time + 1f / attackRate;
        }

        if (damaged && !isDead)
        {
            damageTimer = Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damaged = false;
                damageTime = 0;
            }
        }
        //Debug.Log(damaged);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (!isGrounded)
            {
                v = 0;
            }

            rb.velocity = new Vector3(h * speed, rb.velocity.y, v * speed);

            if (isGrounded)
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

            if (h > 0 && !facingRight)
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }
        }

        Boundaries();
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Boundaries()
    {
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
        /*
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1), 
                                  rb.position.y, 
                                  Mathf.Clamp(rb.position.z, minHeight, maxHeight));
                                  */
    }

    public void ZeroSpeed()
    {
        speed = 0;
    }

    public void ResetSpeed()
    {
        speed = maxSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (lives > 0)
        {
            if (!isDead)
            {
                health -= damage;
                healthBar.SetHealth(health);
                damaged = true;
                anim.SetTrigger("HitDamage");

                if (health <= 0)
                {
                    health = maxHealth;
                    lives -= 1;
                    healthBar.SetText("x" + lives.ToString());
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
    }
}
