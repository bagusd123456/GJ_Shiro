using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCMovement;

[RequireComponent(typeof(NPCMovement))]
public class Mob_Slime : MonoBehaviour
{
    NPCMovement movement;

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

    private void OnValidate()
    {
        if(movement == null)
            movement = GetComponent<NPCMovement>();
    }
    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
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

        inRange = Physics2D.Raycast(transform.position, -transform.right, distance, LayerMask.GetMask("Wall"));
        if (inRange)
            _state = state.ATTACKING;
        else
            _state = state.PATROL;

        Movement();
    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                movement.movementSpeed = 0f;
                break;

            case state.PATROL:
                movement.movementSpeed = 1f;
                break;

            case state.HOSTILE:
                movement.movementSpeed = -1f;
                break;

            case state.DASHING:
                movement.movementSpeed = 2f;
                break;

            case state.ATTACKING:
                movement.movementSpeed = 0f;
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
        var GO = Instantiate(prj, transform.position + -transform.right * distanceFromUser, Quaternion.identity, transform.parent);
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
        Vector3 dir = transform.parent.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    private void OnDrawGizmos()
    {
        if(!inRange)
            Debug.DrawRay(transform.position, -transform.right * distance, Color.white);
        else
            Debug.DrawRay(transform.position, -transform.right * distance, Color.red);

        Gizmos.DrawWireSphere(GetPosition(CurrentAngle() + distanceFromUser, movement.targetDistance), 0.2f);
    }
}
