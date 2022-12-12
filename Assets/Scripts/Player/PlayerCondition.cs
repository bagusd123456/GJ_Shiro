using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : CharacterBase
{
    public void SetupStats()
    {
        currentHP = statsData.HP;
        currentMP = statsData.MP;

        moveSpeed = statsData.moveSpeed;
        attackDamage = statsData.attackDamage;
        attackSpeed = statsData.attackSpeed;
        armor = statsData.armor;
    }

    private void Awake()
    {
        if(loadStatsOnStart)
            SetupStats();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
