using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;


    private Animator animator;
    private Rigidbody2D body;


    float teleportDelay = 4f;
    float teleportCurrentDelay = 1f;
    float teleportChannelDuration = 1f;
    float teleportChannel;


    Vector2 ForceToApply;
    float forceDamping = 1.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (EnemyData.Teleporting)
        {
            
        }
        else
            Move(EnemyData.Speed);
    }

    public void Move(float speed)
    {

        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
            return;
        // Movement speed of enemy
        var step = speed * Time.deltaTime;
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

    public void Teleport()
    {

        animator.SetBool("isTeleporting", true);
        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
        {
            if (teleportCurrentDelay > 0 || teleportChannel > 0)
            {
                if (teleportChannel > 0)
                {
                    teleportChannel -= teleportChannelDuration * Time.deltaTime;
                    if (teleportChannel <= 0)
                    {
                        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

                        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
                        if (roadPath == null && Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
                            return;
                        var tilesize = 0.5f;

                        // If there is a viable path and the enemy is atleast 1 unit away from the player
                        // Draw the path and move the enemy towards the next node in the path
                        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f && roadPath != null)
                        {
                            Vector3 nextNode;
                            if (roadPath.Capacity > 5)
                                nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[3]);
                            else
                                nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
                            nextNode.x += tilesize;
                            nextNode.y += tilesize;
                            transform.position = nextNode;
                        }
                        // If the enemy is closer than 1 unit to the player
                        // Move the enemy directly towards the player position
                        else
                        {
                            Step(Events.GetPlayerPosition());
                        }
                        teleportCurrentDelay = teleportDelay;
                    }
                }
                else
                    teleportCurrentDelay -= teleportDelay * Time.deltaTime;
            }
            else
            {
                teleportChannel = teleportChannelDuration;
            }
            return;
        }

        Move(EnemyData.Speed);

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

        animator.SetFloat("X", xInput);
        animator.SetFloat("Y", yInput);
        animator.SetBool("isWalking", true);
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


}
