using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;


    private Animator animator;
    private string currentAnimationState;

    private Rigidbody2D body;


    float teleportDelay = 2f;
    float teleportChannelDuration;
    bool isChannelingTeleport = false;
    bool isDelayingTeleport = false;


    Vector2 ForceToApply;
    float forceDamping = 1.2f;


    // ANIMATION STATES
    const string ENEMY_IDLE = "Enemy_Idle";
    const string ENEMY_MOVEUP = "Enemy_MoveUp";
    const string ENEMY_MOVEDOWN = "Enemy_MoveDown";
    const string ENEMY_MOVELEFT = "Enemy_MoveLeft";
    const string ENEMY_MOVERIGHT = "Enemy_MoveRight";
    const string ENEMY_TELEPORTCHANNEL = "Enemy_Teleport_Channel";


    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (EnemyData.Teleporting)
            Teleport();
        else
            Move();
    }

    public void Move()
    {
        
        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
            return;
        var tilesize = 0.5f;

        // If there is a viable path and the enemy is atleast 1 unit away from the player
        // Move the enemy towards the next node in the path
        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f && roadPath != null)
        {
            Vector3 nextNode;
            nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
            nextNode.x += tilesize;
            nextNode.y += tilesize;

            Step(nextNode);

        }
        // If the enemy is closer than 1 unit to the player
        // Move the enemy directly towards the player position
        else
        {
            Step(Events.GetPlayerPosition());
        }
    }

    private void Teleported()
    {
        isChannelingTeleport = false;

        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
            return;
        var tilesize = 0.5f;

        // If there is a viable path and the enemy is atleast 1 unit away from the player
        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f && roadPath != null)
        {
            Vector3 nextNode;
            if (roadPath.Count > 5)
                nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[roadPath.Count-2]);
            else
                nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
            nextNode.x += tilesize;
            nextNode.y += tilesize;
            transform.position = nextNode;
        }

        isDelayingTeleport = true;
        Invoke(nameof(ReadyToTeleport), teleportDelay);
    }
    private void ReadyToTeleport()
    {
        isDelayingTeleport = false;
    }
    public void Teleport()
    {

        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 3f)
        {
            if (!isChannelingTeleport && !isDelayingTeleport)
            {
                isChannelingTeleport = true;
                return;
            } 
            else if (isChannelingTeleport && !isDelayingTeleport)
            {
                isDelayingTeleport = true;
                ChangeAnimationState(ENEMY_TELEPORTCHANNEL);
                teleportChannelDuration = animator.GetCurrentAnimatorStateInfo(0).length;
                Invoke(nameof(Teleported), teleportChannelDuration);
                return;
            }
            else if (isChannelingTeleport && isDelayingTeleport)
            {
                // Do nothing
            }
            else
            {
                Move();
                return;
            }
            
        }
        else
        {
            Step(Events.GetPlayerPosition());
        }

    }

    // Steps towards the next position
    private void Step(Vector3 targetPos)
    {
        float xInput = targetPos.x - transform.position.x;
        float yInput = targetPos.y - transform.position.y;
        Vector2 MoveInput = new Vector2(xInput, yInput).normalized;
        Vector2 MoveForce = MoveInput * EnemyData.Speed;
        MoveForce += ForceToApply;
        ForceToApply /= forceDamping;

        if (Mathf.Abs(ForceToApply.x) <= 0.01f && Mathf.Abs(ForceToApply.y) <= 0.01f)
            ForceToApply = Vector2.zero;
        body.velocity = MoveForce;

        if (MoveInput.y < 0 && Mathf.Abs(yInput) > 1)
            ChangeAnimationState(ENEMY_MOVEDOWN);
        else if (MoveInput.y > 0 && Mathf.Abs(yInput) > 1)
            ChangeAnimationState(ENEMY_MOVEUP);
        else if (MoveInput.x < 0)
            ChangeAnimationState(ENEMY_MOVELEFT);
        else if (MoveInput.x > 0)
            ChangeAnimationState(ENEMY_MOVERIGHT);
        else
            ChangeAnimationState(ENEMY_IDLE);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Events.SetHealth(Events.GetHealth() - EnemyData.Damage);
            Vector2 difference = transform.position - Events.GetPlayerPosition();
            difference = difference.normalized * EnemyData.KnockbackAmount; ;
            ForceToApply = difference;
        }
    }


    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

}
