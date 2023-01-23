using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target;
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                if (rangeChecks[i].GetComponent<PlayerCondition>() != null)
                {
                    target = rangeChecks[i].transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.right, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                            canSeePlayer = true;
                        else
                            canSeePlayer = false;
                    }
                    else
                        canSeePlayer = false;
                }
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.right * 3f);
    }
}
