using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable/Stats Character")]
public class StatsData : ScriptableObject
{
    public string characterName;
    [Space]
    [Header("Character Base Stats")]
    public int HP;
    public int MP;
    [Space]
    public float moveSpeed;
    public int attackDamage;
    public float attackSpeed;
    public int armor;

    public void Reset()
    {
        characterName = "Player";

        HP = 100;
        MP = 100;
        moveSpeed = 2;
        attackDamage = 5;
        attackSpeed = 2;
        armor = 10;
    }
}
