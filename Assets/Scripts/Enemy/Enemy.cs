using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;

    private GameObject PathFinder;

    private Animator animator;
    private string currentAnimationState;

    private Rigidbody2D body;
    private Health health;


    float teleportDelay = 2f;
    float teleportChannelDuration;
    bool isChannelingTeleport = false;
    bool isDelayingTeleport = false;

    private List<Spot> roadPath;
    private Vector3 nextNode;

    Vector2 ForceToApply;
    float forceDamping = 1.2f;

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


    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.SetHealth(EnemyData.Health);

        PathFinder = GameObject.Find("Pathfinder");
    }

    private void Start()
    {
        if (health != null)
            health.SetHealth(EnemyData.Health);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) > 1f)
        {
            if (EnemyData.Teleporting)
                Teleport();
            else
                Move();
        }
        else
            Step(Events.GetPlayerPosition());
        
    }

    private void GetRoadPath()
    {
        roadPath = PathFinder.GetComponent<PathFinding>().GetPath(transform.position);
    }

    public void Move()
    {

        if (roadPath == null)
            GetRoadPath();
        if (nextNode != null)
        {
            if (!IsDistanceLonger(transform.position, nextNode, 0.05f))
            {
                GetRoadPath();
            }
        }


        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && IsDistanceLonger(transform.position, Events.GetPlayerPosition(), 1f))
            return;

        var tilesize = 0.5f;

        nextNode = PathFinder.GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
        nextNode.x += tilesize;
        nextNode.y += tilesize;

        Step(nextNode);

    }

    private void Teleported()
    {
        isChannelingTeleport = false;

        if (roadPath == null)
            GetRoadPath();
        if (nextNode != null)
        {
            if (!IsDistanceLonger(transform.position, nextNode, 0.1f))
            {
                GetRoadPath();
            }
        }

        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && IsDistanceLonger(transform.position, Events.GetPlayerPosition(), 1f))
            return;
        var tilesize = 0.5f;

        if (roadPath.Count > 5)
            nextNode = PathFinder.GetComponent<PathFinding>().GetTilemapCoords(roadPath[roadPath.Count - 2]);
        else
            nextNode = PathFinder.GetComponent<PathFinding>().GetTilemapCoords(roadPath[1]);
        nextNode.x += tilesize;
        nextNode.y += tilesize;
        transform.position = nextNode;


        isDelayingTeleport = true;
        Invoke(nameof(ReadyToTeleport), teleportDelay);
    }
    private void ReadyToTeleport()
    {
        isDelayingTeleport = false;
    }
    public void Teleport()
    {

        if (IsDistanceLonger(transform.position, Events.GetPlayerPosition(), 3f))
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
        MoveInput = new Vector2(xInput, yInput).normalized;


        MoveForce = MoveInput * EnemyData.Speed;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Events.SetHealth(Events.GetHealth() - EnemyData.Damage);
            Vector2 difference = transform.position - Events.GetPlayerPosition();
            difference = difference.normalized * EnemyData.KnockbackAmount;
            ForceToApply = difference;
        }
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


    bool IsDistanceLonger(Vector3 pos1, Vector3 pos2, float distanceCompare)
    {
        Vector3 offset = pos2 - pos1;
        if (offset.sqrMagnitude > Mathf.Pow(distanceCompare, 2))
            return true;
        else
            return false;
    }

}
