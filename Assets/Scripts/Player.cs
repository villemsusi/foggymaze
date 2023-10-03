using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Player : MonoBehaviour
{
    Rigidbody2D body;
    public static Player Instance;

    float horizontal;
    float vertical;

    // Set movement speed of player
    private float moveSpeed = 5.0f;


    public VisualEffect vfxRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Debug.Log(transform.position);
        vfxRenderer.SetVector3("ColliderPos", transform.position);
    }

    private void FixedUpdate()
    {
        // Velocity based on input and movespeed
        body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        // Adjust velocity for diagonal movement
        if (horizontal != 0 && vertical != 0)
            body.velocity *= 0.7f;
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
