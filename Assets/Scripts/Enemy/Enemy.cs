using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;

    private Animator animator;
    private string currentAnimationState;

    private CameraShake shake;

    private Rigidbody2D body;
    private Health health;


    float teleportDelay = 2f;
    float teleportChannelDuration;
    bool isChannelingTeleport = false;
    bool isDelayingTeleport = false;


    Vector2 ForceToApply;
    float forceDamping = 1.2f;

    private float hitCooldown = 0f;

    Vector2 MoveForce;
    Vector2 MoveInput;

    bool inWall = false;


    // ANIMATION STATES
    const string ENEMY_IDLE = "Enemy_Idle";
    const string ENEMY_MOVEUP = "Enemy_MoveUp";
    const string ENEMY_MOVEDOWN = "Enemy_MoveDown";
    const string ENEMY_MOVELEFT = "Enemy_MoveLeft";
    const string ENEMY_MOVERIGHT = "Enemy_MoveRight";
    const string ENEMY_TELEPORTCHANNEL = "Enemy_Teleport_Channel";



    public NavMeshAgent agent;
    NavMeshPath path;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        shake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start()
    {
        if (health != null)
            health.SetHealth((int)(EnemyData.Health * Events.GetEnemyHealthFactor()));

        path = new NavMeshPath();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.CalculatePath(Events.GetPlayerPosition(), path);
    }

    private void Update()
    {
        if (hitCooldown > 0)
            hitCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Is closer to player than 2 tiles?
        if (!IsDistanceToPosLonger(Events.GetPlayerPosition(), 1f))
        {
            Step(Events.GetPlayerPosition());
        }
        else if (!IsDistanceToPosLonger(Events.GetPlayerPosition(), 16f))
            agent.CalculatePath(Events.GetPlayerPosition(), path);
        else
        {
            if (!IsDistanceToPosLonger(agent.pathEndPosition, 4f))
                agent.CalculatePath(Events.GetPlayerPosition(), path);
        }
        if (path.corners.Length != 0) Step(path.corners[1]);
        else Step(Events.GetPlayerPosition());
    }

    // Steps towards the next position
    private void Step(Vector3 targetPos)
    {
        float xInput = targetPos.x - transform.position.x;
        float yInput = targetPos.y - transform.position.y;
        MoveInput = new Vector2(xInput, yInput).normalized;


        MoveForce = MoveInput * EnemyData.Speed * Events.GetEnemyMovespeedFactor();
        if (!inWall)
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


    private void Teleported()
    {
        isChannelingTeleport = false;


        isDelayingTeleport = true;
        Invoke(nameof(ReadyToTeleport), teleportDelay);
    }
    private void ReadyToTeleport()
    {
        isDelayingTeleport = false;
    }
    public void Teleport()
    {

        if (IsDistanceToPosLonger(Events.GetPlayerPosition(), 3f))
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

        }
        else
        {
            Step(Events.GetPlayerPosition());
        }

    }



    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    public void SetForce(Vector3 force)
    {
        ForceToApply += new Vector2(force.x, force.y);
    }


    bool IsDistanceToPosLonger(Vector3 target, float distanceCompare)
    {
        Vector3 offset = target - transform.position;
        if (offset.sqrMagnitude > Mathf.Pow(distanceCompare, 2))
            return true;
        else
            return false;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (hitCooldown <= 0)
            {
                Events.SetHealth(Events.GetHealth() - EnemyData.Damage);
                hitCooldown = 0.3f;
                Events.SetTrauma(Events.GetTrauma() + 0.5f);
            }
            
            Vector2 difference = transform.position - Events.GetPlayerPosition();
            difference = difference.normalized * EnemyData.KnockbackAmount;
            ForceToApply = difference;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>() != null)
        {
            inWall = true;
            ForceToApply = Vector2.zero;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>() != null)
        {
            inWall = false;
        }
    }

}
