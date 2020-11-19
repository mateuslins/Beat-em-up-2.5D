using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform player;

    private bool isGrounded;
    private bool isDead = false;
    private bool facingRight = true;
    private bool damaged = false;
    private float speed;
    private float zForce;
    private float walkTimer;
    private float damageTimer;
    private float health;
    private float nextAttack;

    public float maxSpeed = 2;
    public float minHeight = -9;
    public float maxHeight = 4;
    public float damageTime = 0.5f;
    public float attackRate = 1f;
    public int maxHealth = 60;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        speed = maxSpeed;
        health = maxHealth;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Dead", isDead);

        if (!isDead)
        {
            FacePlayer();
        }

        walkTimer += Time.deltaTime;

        if (damaged && !isDead)
        {
            damageTimer = Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damaged = false;
                damageTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 playerDistance = player.position - transform.position;

            ChasePlayer(playerDistance);

            if (isGrounded)
                anim.SetFloat("Speed", Mathf.Abs(speed));

            AttackPlayer(playerDistance);
        }
    }

    private void FacePlayer()
    {
        facingRight = (transform.position.x < player.position.x) ? false : true;

        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void ChasePlayer(Vector3 playerDistance)
    {
        // Chase horizontaly
        float hForce = playerDistance.x / Mathf.Abs(playerDistance.x);

        // Chase verticaly with random variance
        if (walkTimer >= Random.Range(1f, 2f))
        {
            zForce = Random.Range(-1, 2);
            walkTimer = 0;
        }

        // Stop chasing if is in range
        if (Mathf.Abs(playerDistance.x) < 1f)
        {
            hForce = 0;
        }

        rb.velocity = new Vector3(hForce * speed, 0, zForce * speed);
    }

    private void AttackPlayer(Vector3 playerDistance)
    {
        if (Mathf.Abs(playerDistance.x) < 1f && Mathf.Abs(playerDistance.z) < 1f && Time.time > nextAttack)
        {
            anim.SetTrigger("Attack");
            nextAttack = Time.time + attackRate;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            damaged = true;
            health -= damage;
            anim.SetTrigger("HitDamage");

            if (health <= 0)
            {
                isDead = true;
                rb.AddRelativeForce(new Vector3(-3, 5, 0), ForceMode.Impulse);
                anim.SetFloat("Speed", 0f);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    private void Boundaries()
    {
        /*
        rb.position = new Vector3(rb.position.x,
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
