using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;

    public EnemyData EnemyData;


    float teleportDelay = 4f;
    float teleportCurrentDelay = 1f;
    float teleportChannelDuration = 1f;
    float teleportChannel;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (EnemyData.Teleporting)
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
                            Teleport(EnemyData.Speed);
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
            }
            else
            {
                Move(EnemyData.Speed);
            }
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
            transform.position = Vector3.MoveTowards(transform.position, nextNode, step);

            animator.SetFloat("X", nextNode.x - transform.position.x);
            animator.SetFloat("Y", nextNode.y - transform.position.y);
            animator.SetBool("isWalking", true);
        }
        // If the enemy is closer than 1 unit to the player
        // Move the enemy directly towards the player position
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Events.GetPlayerPosition(), step);
        }
    }

    public void Teleport(float speed)
    {
        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
            return;
        // Movement speed of enemy
        var step = speed * Time.deltaTime;
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


            animator.SetFloat("X", nextNode.x - transform.position.x);
            animator.SetFloat("Y", nextNode.y - transform.position.y);

            animator.SetBool("isWalking", true);
        }
        // If the enemy is closer than 1 unit to the player
        // Move the enemy directly towards the player position
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Events.GetPlayerPosition(), step);
        }
    }


}
