using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Crystal : MonoBehaviour
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
    float time;
    public float timeCooldown = 0.8f;
    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        fov = GetComponent<FieldOfView>();
    }

    private void OnDisable()
    {
        movement.enabled = false;
    }

    private void OnEnable()
    {
        movement.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;

        if (!GameManager.Instance.player.isDead)
        {
            if (_state == state.DASHING || _state == state.ATTACKING)
            {
                Collider[] hit = Physics.OverlapSphere(transform.position, closeDistance, fov.targetMask);
                if (hit.Length > 0)
                    collide = true;
                else
                    collide = false;
            }

            else if (_state == state.PATROL)
            {
                inRange = fov.canSeePlayer;

                if (inRange)
                    StartCoroutine(FallbackBehavior());
            }

            Movement();
        }
    }

    public IEnumerator FallbackBehavior()
    {
        inRange = false;
        _state = state.IDLE;
        yield return new WaitForSeconds(.5f);
        _state = state.HOSTILE;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(DashAttack());
    }

    public IEnumerator DashAttack()
    {
        //bool collide = Physics2D.Raycast(transform.position,-transform.right,3f, LayerMask.GetMask("Wall")); Gak bisa raycast di Coroutine
        _state = state.DASHING;
        yield return new WaitUntil(() => collide);
        collide = false;
        _state = state.IDLE;
        yield return new WaitForSeconds(1f);
        _state = state.ATTACKING;
    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                movement.currentMovementSpeed = 0f;
                break;

            case state.PATROL:
                movement.currentMovementSpeed = movement.normalMovementSpeed;
                break;

            case state.HOSTILE:
                movement.currentMovementSpeed = -movement.hostileMovementSpeed;
                break;

            case state.DASHING:
                movement.currentMovementSpeed = movement.dashMovementSpeed;
                break;

            case state.ATTACKING:
                StartCoroutine(MeleeAttack());
                break;
        }
    }

    public IEnumerator MeleeAttack()
    {
        if (collide)
        {
            if (time <= 0)
            {
                Debug.Log("Attack Player");
                time = timeCooldown;
            }
        }
        else
        {
            _state = state.IDLE;
            yield return new WaitForSeconds(0.5f);
            _state = state.PATROL;
        }
    }

    private void OnDrawGizmos()
    {
        if(!inRange)
            Debug.DrawRay(transform.position, transform.right * distance, Color.white);
        else
            Debug.DrawRay(transform.position, transform.right * distance, Color.red);

        if (!collide)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, closeDistance);
        }

        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closeDistance);
        }
    }
}
