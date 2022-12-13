using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;
using static PlayerCondition;

[RequireComponent(typeof(PlayerCondition))]
public class PlayerAttack : MonoBehaviour
{
    PlayerCondition _char;

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

    private void Awake()
    {
        _char = GetComponent<PlayerCondition>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAttackTime = _char.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAttackTime < timeBetweenAttack / _char.attackSpeed)
        {
            currentAttackTime += Time.deltaTime;
            _char._state = State.BUSY;
        }
        else if(currentAttackTime > timeBetweenAttack / _char.attackSpeed && _char._state != State.DEFENDING)
            _char._state = State.IDLE;

        //Combo Attack Time
        if (currentComboTime < cdCombo + timeBetweenAttack)
            currentComboTime += Time.deltaTime;
        else
        {
            comboIndex = 0;
        }
    }

    public void BasicAttack()
    {
        if(_char._state == State.IDLE && !_char.isDead)
        {
            currentAttackTime = 0;
            currentComboTime = 0;
            var GO = Instantiate(prj);
            if (gameObject.GetComponent<PlayerMovement>()._rotateDir == rotateDir.RIGHT)
            {
                GO.currentAngle = CurrentAngle() + distanceFromPlayer;
                GO.inverseRotation = false;
            }

            else
            {
                GO.currentAngle = CurrentAngle() - distanceFromPlayer;
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
        if(_char._state == State.IDLE && !_char.isDead)
        {
            _char._state = State.DEFENDING;
            comboIndex = 0;
            Debug.Log("Defense");
            yield return new WaitForSeconds(cdDef);

            _char._state = State.IDLE;
        }
    }

    float CurrentAngle()
    {
        Vector3 dir = center.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.forward + Vector3.left * distanceFromPlayer,0.2f);
    }
}
