using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Crystal : MonoBehaviour
{
    NPCMovement movement;

    public Vector3 target;
    bool inRange;
    public float distance;
    public float closeDistance;

    public enum state { IDLE, PATROL, HOSTILE, ATTACKING, FALLBACK}
    public state _state = state.IDLE;

    float time;
    public float timeCooldown = 0.8f;
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

        if (inRange && _state == state.PATROL)
        {
            StartCoroutine(FallbackBehavior());
        }

        Movement();
    }

    public IEnumerator FallbackBehavior()
    {
        _state = state.IDLE;
        yield return new WaitForSeconds(.5f);
        _state = state.HOSTILE;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(AttackBehavior());
    }

    public IEnumerator AttackBehavior()
    {
        bool collide = Physics2D.Raycast(transform.position,-transform.right,3f, LayerMask.GetMask("Wall"));
        _state = state.ATTACKING;
        yield return new WaitUntil(()=> collide || inRange);
        _state = state.IDLE;
        yield return new WaitForSeconds(1f);
        _state = state.PATROL;

    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                movement.moveDir = 0;
                break;
            case state.PATROL:
                movement.moveDir = 1;
                movement.movementSpeed = 1f;
                break;

            case state.HOSTILE:
                movement.moveDir = -1;
                break;

            case state.ATTACKING:
                movement.moveDir = 1;
                movement.movementSpeed = 2f;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if(!inRange)
            Debug.DrawRay(transform.position, -transform.right * distance, Color.white);
        else
            Debug.DrawRay(transform.position, -transform.right * distance, Color.red);
    }
}
