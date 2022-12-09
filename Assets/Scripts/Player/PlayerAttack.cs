using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum State { IDLE, BUSY};
    public State _state = State.IDLE;

    float currentComboTime;
    public float cdCombo = 1.5f;
    int comboIndex = 0;

    public float cdDef = 1f;
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
        if(_state == State.IDLE)
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

    public IEnumerator Defend()
    {
        if(_state == State.IDLE)
        {
            _state = State.BUSY;
            comboIndex = 0;
            Debug.Log("Defense");
            yield return new WaitForSeconds(cdDef);

            _state = State.IDLE;
        }
    }
}
