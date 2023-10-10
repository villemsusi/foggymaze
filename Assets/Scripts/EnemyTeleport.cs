using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleport : MonoBehaviour
{

    private Enemy Instance;

    float speed = 2;
    float health = 5;

    float teleportDelay = 4f;
    float teleportCurrentDelay = 1f;

    float teleportChannelDuration;
    float teleportChannel;

    private Animator animator;

    private void Awake()
    {
        Instance = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        teleportChannelDuration = teleportDelay / 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportCurrentDelay > 0 || teleportChannel > 0) 
        {
            if (teleportChannel > 0)
            {
                teleportChannel -= teleportChannelDuration * Time.deltaTime;
                if (teleportChannel <= 0)
                {
                    animator.SetBool("isTeleporting", false);
                    Instance.Move(speed, true);
                    //animator.SetBool("isWalking", true);
                    
                    teleportCurrentDelay = teleportDelay;
                }
            } else
            {
                Instance.Move(speed);
                
                teleportCurrentDelay -= teleportDelay * Time.deltaTime;
            }
            
        } else
        {
            teleportChannel = teleportChannelDuration;
            //animator.SetBool("isWalking", false);
            animator.SetBool("isTeleporting", true);
        }

        Instance.HealthCheck(health);
    }
}
