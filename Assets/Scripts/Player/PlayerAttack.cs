using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float currentComboTime;
    public float cdCombo = 1.5f;
    public int comboIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentComboTime < 1)
            currentComboTime += Time.deltaTime;
        else
            comboIndex = 0;
    }

    public void BasicAttack()
    {
        currentComboTime = 0;
        switch (comboIndex)
        {
            case 0:
                Debug.Log("Attack 1");
                comboIndex++;
                break;
            case 1:
                Debug.Log("Attack 2");
                comboIndex++;
                break;
            case 2:
                Debug.Log("Attack 3");
                comboIndex++;
                break;
            default:
                break;
        }
    }
}
