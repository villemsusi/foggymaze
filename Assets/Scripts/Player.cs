using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;

    public static Player Instance;

    float horizontal;
    float vertical;

    // Set movement speed of player
    private float moveSpeed = 4.0f;


    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        // Velocity based on input and movespeed
        body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
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


        // Adjust velocity for diagonal movement
        if (horizontal != 0 && vertical != 0)
            body.velocity *= 0.7f;

        if (Input.GetMouseButtonDown(0))
            Game.Instance.ExpandFog();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Game.Instance.RestartGame();
        }
    }
}
