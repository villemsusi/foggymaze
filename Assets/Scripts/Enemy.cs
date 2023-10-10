using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D body;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(float speed, bool teleport = false)
    {

        List<Spot> roadPath = GetComponent<PathFinding>().GetPath();

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Player.Instance.transform.position) > 1f)
            return;
        // Movement speed of enemy
        var step = speed * Time.deltaTime;
        var tilesize = 0.5f;

        // If there is a viable path and the enemy is atleast 1 unit away from the player
        // Draw the path and move the enemy towards the next node in the path
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 1f && roadPath != null)
        {
            Vector3 nextNode;
            if (teleport)
            {
                
                if (roadPath.Capacity > 5)
                    nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[3]);
                else
                    nextNode = GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
                nextNode.x += tilesize;
                nextNode.y += tilesize;
                transform.position = nextNode;
                return;
            }
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
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, step);
        }
    }

    public void HealthCheck(float health)
    {
        if (health == 0)
        {
            gameObject.SetActive(false);
        }
    }


}
