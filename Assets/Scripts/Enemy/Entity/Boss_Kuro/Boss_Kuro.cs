using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCMovement;

public class Boss_Kuro : MonoBehaviour
{
    bool inRange;
    public PlayerCondition player;
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
        
    }
    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCondition>();
        player = FindObjectOfType<PlayerCondition>();
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
    }

    public void Movement()
    {
        switch (_state)
        {
            case state.IDLE:
                
                break;

            case state.PATROL:
                
                break;

            case state.HOSTILE:
                
                break;

            case state.DASHING:
                
                break;

            case state.ATTACKING:
                
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
        Debug.DrawRay(transform.position, transform.right * 2f);
    }
}
