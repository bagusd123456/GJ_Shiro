using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public bool loadStatsOnStart = true;
    public StatsData statsData;

    [Header("Character Status")]
    public int currentHP;
    public int currentMP;
    [Space]
    public float moveSpeed;
    public int attackDamage;
    public float attackSpeed;
    public int armor;

    public bool isDead;

    Animator _animator;
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        if (loadStatsOnStart)
            SetupStats();
    }

    // Update is called once per frame
    public void Update()
    {
        OnDead();
    }

    public void SetupStats()
    {
        currentHP = statsData.HP;
        currentMP = statsData.MP;

        moveSpeed = statsData.moveSpeed;
        attackDamage = statsData.attackDamage;
        attackSpeed = statsData.attackSpeed;
        armor = statsData.armor;
    }

    void OnDead()
    {
        if (currentHP <= 0)
        {
            this.isDead = true;
            currentHP = 0;
        } 
    }

    void OnMove()
    {
        if (_animator != null)
            _animator.SetBool("isWalking",true);
    }

    public void TakeDamage(int damage)
    {
        if(!isDead)
        currentHP -= damage;
    }
}
