using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesForward : MonoBehaviour
{
    PlayerMovement player;
    [HideInInspector]
    public Transform center;
    Vector3 offset;
    public float targetDistance = 3;
    public float angle;
    public float projectileSpeed = 1f;
    [Space]
    public int damageAmount;

    public bool canMove = false;
    
    private void Awake()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        center = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            transform.position += transform.right * projectileSpeed * Time.deltaTime;

        LookAtTarget();
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = center.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle - 180, 2f));
    }

    public void SetPosition()
    {
        offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
        transform.position = center.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage every Character Collide
        if(other.GetComponent<CharacterBase>() != null)
        {
            other.GetComponent<CharacterBase>().TakeDamage(damageAmount);
            
            Destroy(this.gameObject);
        }
    }
}
