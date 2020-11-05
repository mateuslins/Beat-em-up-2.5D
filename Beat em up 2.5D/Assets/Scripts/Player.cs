using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField]
    private Transform groundCheck;

    private bool isGrounded;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;
    private float speed;
    private float nextAttackTime;

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight = -9;
    public float maxHeight = 4;
    public float attackRate = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        speed = maxSpeed;
        isGrounded = true;
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

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
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

            Boundaries();
        }
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
