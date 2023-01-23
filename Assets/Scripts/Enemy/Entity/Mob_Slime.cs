using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCMovement;

[RequireComponent(typeof(NPCMovement))]
public class Mob_Slime : MonoBehaviour
{
    NPCMovement movement;
    FieldOfView fov;

    bool inRange;
    public float distance;
    public bool collide;
    public float closeDistance;

    public enum state { IDLE, PATROL, HOSTILE, DASHING, ATTACKING}
    public state _state = state.IDLE;

    [Header("Attack Parameter")]
    public Projectiles prj;
    public Transform spawnTarget;

    float time;
    public float timeCooldown = 0.8f;
    public float distanceFromUser;

    public Vector3 offset;

    Animator animator;
    private void OnValidate()
    {
        if(movement == null)
            movement = GetComponent<NPCMovement>();
    }
    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        fov = GetComponent<FieldOfView>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        movement.enabled = false;
    }

    private void OnEnable()
    {
        movement.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;

        if (!GameManager.Instance.player.isDead)
        {
            inRange = fov.canSeePlayer;
            if (inRange)
                _state = state.ATTACKING;
            else
                _state = state.PATROL;

            Movement();
        }
            
        else
            _state = state.IDLE;
    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                movement.currentMovementSpeed = 0f;
                animator.SetBool("Running", false);
                break;

            case state.PATROL:
                movement.currentMovementSpeed = movement.normalMovementSpeed;
                animator.SetBool("Running", true);
                break;

            case state.HOSTILE:
                movement.currentMovementSpeed = movement.hostileMovementSpeed ;
                break;

            case state.DASHING:
                movement.currentMovementSpeed = movement.dashMovementSpeed;
                break;

            case state.ATTACKING:
                movement.currentMovementSpeed = 0f;
                BasicAttack();
                break;
        }
    }

    public void BasicAttack()
    {
        if (time <= 0)
        {
            SpawnProjectile();
        }
    }

    [ContextMenu("Spawn")]
    public Projectiles SpawnProjectile()
    {
        time = timeCooldown;
        var GO = Instantiate(prj, transform.position + transform.right * distanceFromUser, Quaternion.identity, transform.parent);
        GO.center = transform.parent;
        GO.targetDistance = gameObject.GetComponent<NPCMovement>().targetDistance;

        if (gameObject.GetComponent<NPCMovement>()._rotateDir == rotateDir.RIGHT)
        {
            GO.currentAngle = CurrentAngle() - distanceFromUser;
            GO.inverseRotation = false;
        }

        else
        {
            GO.currentAngle = CurrentAngle() + distanceFromUser;
            GO.inverseRotation = true;
        }
        return GO;
    }

    float CurrentAngle()
    {
        if (transform.parent != null)
        {

            Vector3 dir = transform.parent.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
            return angle;
        }
        else
        {
            return 0;
        }
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    private void OnDrawGizmos()
    {
        Vector3 targetPos = Vector3.zero;
        if(movement._rotateDir == 0)
            targetPos = GetPosition(CurrentAngle() + distanceFromUser, movement.targetDistance);
        else if(movement._rotateDir == (rotateDir)1)
            targetPos = GetPosition(CurrentAngle() - distanceFromUser, movement.targetDistance);

        targetPos.z = transform.position.z;
        Gizmos.DrawWireSphere(targetPos, 0.2f);
    }
}
