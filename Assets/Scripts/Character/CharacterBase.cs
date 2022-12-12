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
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        OnDead();
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
}
