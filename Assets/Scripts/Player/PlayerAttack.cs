using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;
using static PlayerCondition;
using System.Linq;

[RequireComponent(typeof(PlayerCondition))]
public class PlayerAttack : MonoBehaviour
{
    PlayerCondition _char;
    ComboManager _comboManager;

    [Header("Attack Parameter")]
    public Projectiles prj;
    public Transform spawnTarget;
    public float distanceFromPlayer = 15f;
    float currentAttackTime;
    public float timeBetweenAttack = .5f;

    public float currentComboTime;
    public float cdCombo = 1.5f;
    [HideInInspector]
    public int comboIndex = 0;

    public List<int> comboInput = new List<int>(3);

    [Header("Defend Parameter")]
    public float cdDef = 1f;

    public bool canCombo = true;

    private void Awake()
    {
        _char = GetComponent<PlayerCondition>();
        _comboManager = GetComponent<ComboManager>();
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
            for (int i = 0; i < comboInput.Count; i++)
            {
                comboInput[i] = 0;
            }
            
        }
        if (currentComboTime >= 1f && canCombo)
        {
            CheckCombo(comboInput.ToList());
            canCombo = false;
        }
    }

    public void RegisterCombo(int input)
    {
        if(comboIndex < 3 )
        {
            currentComboTime = 0;
            comboInput[comboIndex] = input;
            comboIndex++;
            canCombo = true;
        }
        
    }

    public void CheckCombo(List<int> input)
    {
        for (int i = 0; i < _comboManager.comboMove.Count; i++)
        {
            if (ComboManager.CountMatches(_comboManager.comboMove[i].comboMove, comboInput) == comboInput.Count
                )
                _comboManager.Attack(i);
        }
    }

    public void BasicAttack()
    {

        if(_char._state == State.IDLE && !_char.isDead && !_char.isTired)
        {
            PlayerCondition.Instance.UseMana(10);
            currentAttackTime = 0; //Time needed before next Attack

            SpawnProjectile();
        }
    }

    public Projectiles SpawnProjectile()
    {
        PlayerMovement.Instance.animator.SetTrigger("Attack");

        var GO = Instantiate(prj, spawnTarget.forward * distanceFromPlayer, Quaternion.identity, transform.parent);
        GO.player = PlayerMovement.Instance;
        GO.center = PlayerMovement.Instance.center;

        if (gameObject.GetComponent<PlayerMovement>()._rotateDir == rotateDir.RIGHT)
        {
            GO.currentAngle = CurrentAngle() + distanceFromPlayer;
            GO.RotationSet();
            GO.inverseRotation = false;

        }

        else
        {
            GO.currentAngle = CurrentAngle() - distanceFromPlayer;
            GO.RotationSet();
            GO.inverseRotation = true;
        }
        return GO;
    }

    public IEnumerator Defend()
    {
        if(!_char.isDead)
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
        Vector3 dir = PlayerMovement.Instance.center.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return angle;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.forward + Vector3.left * distanceFromPlayer,0.2f);
    }
}
