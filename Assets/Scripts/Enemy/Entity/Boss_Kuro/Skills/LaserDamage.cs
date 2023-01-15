using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        DamageContinuous(other);
    }

    public void DamageContinuous(Collider other)
    {
        if (other.GetComponent<PlayerCondition>() != null)
        {
            Debug.Log("Player Take Damage");
            other.GetComponent<PlayerCondition>().TakeDamage(5);
        }
    }
    public void DisableGO()
    {
        gameObject.SetActive(false);
    }
}
