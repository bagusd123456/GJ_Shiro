using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Base : MonoBehaviour
{
    float timer;
    public float duration;
    public enum type { PASSIVE, ACTIVE}
    public type _type;
    
    void Update()
    {
        if (duration < timer)
        {
            TriggerEffect();
            timer += Time.deltaTime;
        }
    }

    public void TriggerEffect()
    {

    }
}
