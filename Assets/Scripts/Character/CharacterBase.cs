using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public bool loadStatsOnStart = true;
    public StatsData statsData;

    [Header("Character Status")]
    private int MaxHP;
    public int currentHP;
    private int MaxMP;
    public int currentMP;
    [Space]
    public float moveSpeed;
    public int attackDamage;
    public float attackSpeed;
    public int armor;

    public bool isDead;
    public bool isTired;

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
        OnTired();
    }

    public void SetupStats()
    {
        MaxHP = statsData.HP;
        currentHP = MaxHP;
        
        MaxMP = statsData.MP;
        currentMP = MaxMP;

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

    public void GetHealth(int health)
    {
        if(currentHP != MaxHP)
        {
            currentHP += health;
        }
    }

    public void GetMp(int Mana)
    {
        if (currentMP != MaxMP)
        {
            currentMP += Mana;
        }
    }

    public void UseMana(int ManaLos)
    {
        if(currentMP !>= 0)
        {
            currentMP -= ManaLos;
        }
        
    }
    void OnTired()
    {
        if (currentMP <= 0)
        {
            this.isTired = true;
            currentMP = 0;
        }
        else
        {
            this.isTired = false;
        }
    }
}
