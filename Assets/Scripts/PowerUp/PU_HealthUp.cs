using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_HealthUp : MonoBehaviour,IPowerUp
{

    [ContextMenu("Trigger PowerUp")]
    public void TriggerPowerUp()
    {
        PlayerCondition.Instance.statsData.HP++;
    }
}
