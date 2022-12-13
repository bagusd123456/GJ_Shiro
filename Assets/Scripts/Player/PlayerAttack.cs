using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCondition))]
public class PlayerAttack : MonoBehaviour
{
    CharacterBase _char;
    public enum State { IDLE, ATTACKING, BUSY};
    public State _state = State.IDLE;

    [Header("Attack Parameter")]
    public Projectiles prj;
    public float distanceFromPlayer = 15f;
    public Transform center;
    float currentAttackTime;
    public float timeBetweenAttack = .5f;

    float currentComboTime;
    public float cdCombo = 1.5f;
    int comboIndex = 0;

    [Header("Defend Parameter")]
    public float cdDef = 1f;

    public float currentAngle;
    private void Awake()
    {
        _char = GetComponent<CharacterBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAttackTime = _char.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAttackTime > timeBetweenAttack / _char.attackSpeed)
            _state = State.IDLE;
        else
        {
            currentAttackTime += Time.deltaTime;
            _state = State.BUSY;
        }

        //Combo Attack Time
        if (currentComboTime < cdCombo + timeBetweenAttack)
            currentComboTime += Time.deltaTime;
        else
        {
            comboIndex = 0;
        }

        currentAngle = GetAngle();
    }

    public void BasicAttack()
    {
        if(_state == State.IDLE)
        {
            currentAttackTime = 0;
            currentComboTime = 0;
            var GO = Instantiate(prj);
            if (gameObject.GetComponent<RotateObject>()._rotateDir == RotateObject.rotateDir.RIGHT)
            {
                GO.currentAngle = currentAngle + distanceFromPlayer;
                GO.inverseRotation = false;

            }
            else
            {
                GO.currentAngle = currentAngle - distanceFromPlayer;
                GO.inverseRotation = true;
            }
            switch (comboIndex)
            {
                case 0:
                    //Debug.Log("Attack 1");
                    comboIndex++;
                    break;
                case 1:
                    //Debug.Log("Attack 2");
                    comboIndex++;
                    break;
                case 2:
                    //Debug.Log("Attack 3");
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

    float GetAngle()
    {
        Vector3 dir = center.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }
}
