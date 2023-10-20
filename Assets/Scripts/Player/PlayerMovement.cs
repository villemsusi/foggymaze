using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;


    float horizontal;
    float vertical;
    Vector2 Movement;

    // Set movement speed of player
    private float moveSpeed;
    public Vector2 ForceToApply;
    public float forceDamping;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeed = Events.GetMovespeed();
    }

    private void FixedUpdate()
    {
        // Velocity based on input and movespeed
        Vector2 PlayerInput = new Vector2(horizontal, vertical).normalized;
        Vector2 MoveForce = PlayerInput * moveSpeed;
        MoveForce += ForceToApply;
        ForceToApply /= forceDamping;

        if (Mathf.Abs(ForceToApply.x) <= 0.01f && Mathf.Abs(ForceToApply.y) <= 0.01f)
            ForceToApply = Vector2.zero;
        body.velocity = MoveForce;
    }


    void Update()
    {
        // Get movement input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        Movement.x = horizontal;
        Movement.y = vertical;

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("X", horizontal);
            animator.SetFloat("Y", vertical);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
