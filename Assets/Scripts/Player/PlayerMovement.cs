using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    private Animator animator;
    private string currentAnimationState;


    float horizontal;
    float vertical;
    Vector2 Movement;

    // Set movement speed of player
    private float movespeed;
    public Vector2 ForceToApply;
    public float forceDamping;


    // ANIMATION STATES
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_MOVEUP = "Player_MoveUp";
    const string PLAYER_MOVEDOWN = "Player_MoveDown";
    const string PLAYER_MOVELEFT = "Player_MoveLeft";
    const string PLAYER_MOVERIGHT = "Player_MoveRight";



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Events.OnGetMovespeed += GetMovespeed;
        Events.OnSetMovespeed += SetMovespeed;
    }

    private void OnDestroy()
    {
        Events.OnGetMovespeed -= GetMovespeed;
        Events.OnSetMovespeed -= SetMovespeed;
    }

    private void Start()
    {
        SetMovespeed(Events.GetMovespeedPerm());
    }

    private void FixedUpdate()
    {
        // Velocity based on input and movespeed
        Vector2 PlayerInput = new Vector2(horizontal, vertical).normalized;
        Vector2 MoveForce = PlayerInput * movespeed;
        MoveForce += ForceToApply;
        ForceToApply /= forceDamping;

        if (Mathf.Abs(ForceToApply.x) <= 0.01f && Mathf.Abs(ForceToApply.y) <= 0.01f)
            ForceToApply = Vector2.zero;
        body.velocity = MoveForce;

        if (PlayerInput.y < 0)
            ChangeAnimationState(PLAYER_MOVEDOWN);
        else if (PlayerInput.y > 0)
            ChangeAnimationState(PLAYER_MOVEUP);
        else if (PlayerInput.x < 0)
            ChangeAnimationState(PLAYER_MOVELEFT);
        else if (PlayerInput.x > 0)
            ChangeAnimationState(PLAYER_MOVERIGHT);
        else
            ChangeAnimationState(PLAYER_IDLE);
    }


    void Update()
    {
        // Get movement input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        Movement.x = horizontal;
        Movement.y = vertical;
    }



    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    float GetMovespeed() => movespeed;
    void SetMovespeed(float amount) => movespeed = amount;
}
