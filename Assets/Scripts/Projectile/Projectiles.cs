using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float currentAngle;
    
    public PlayerMovement player;
    public Transform center;
    public float targetDistance;

    public Vector3 offset;
    public float moveSpeed = 1f;
    public bool inverseRotation = false;
    [Space]
    public int damageAmount;
    public bool canMove;
    
    private void Awake()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RotationSet();
    }

    //Get Next Position Based on Angle & Distance
    public static Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    public void RotationSet()
    {
        //Rotate GameObject by Angle
        if (!inverseRotation)
            currentAngle += moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;
        else
            currentAngle += -moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;

        Vector3 targetPos = GetPosition(currentAngle, targetDistance);
        transform.position = center.position + targetPos + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage every Character Collide
        if(other.GetComponent<CharacterBase>() != null)
        {
            if(other.GetComponent<PlayerCondition>() != null)
            {
                if (other.GetComponent<PlayerCondition>()._state != PlayerCondition.State.DEFENDING)
                    other.GetComponent<PlayerCondition>().TakeDamage(damageAmount);
            }
            else
                other.GetComponent<CharacterBase>().TakeDamage(damageAmount);
            
                
            Destroy(this.gameObject);
        }
    }
}
