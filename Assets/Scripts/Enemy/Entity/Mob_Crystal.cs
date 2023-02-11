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

    Animator animator;
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

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;

        if (!GameManager.Instance.player.isDead)
        {

            if (_state == state.PATROL)
            {
                inRange = fov.canSeePlayer;

                if (inRange)
                    _state = state.ATTACKING;
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
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.7f);
        _state = state.ATTACKING;
        
        PlayerCondition.Instance.TakeDamage(1);
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
        if (time <= 0)
        {
            PlayerCondition.Instance.TakeDamage(1);
            yield return new WaitForSeconds(1f);

            time = timeCooldown;
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
