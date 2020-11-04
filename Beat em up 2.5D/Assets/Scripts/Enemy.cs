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
    private float speed;

    public float maxSpeed = 4;

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

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (isGrounded)
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
        }
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
