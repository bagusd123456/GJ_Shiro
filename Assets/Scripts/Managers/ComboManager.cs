using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public List<ComboData> comboMove = new List<ComboData>();
    public PlayerAttack player;
    public void Attack(int i)
    {
        switch (i)
        {
            case 0:
                Combo1();
                break;

            case 1:
                Combo2();
                break;

            case 2:
                Combo3();
                break;

            case 3:
                Combo4();
                break;
            case 4:
                Combo5();
                break;
        }

    }

    public void Combo1()
    {
        player.BasicAttack();
        Debug.Log("Attack 1");
    }

    public void Combo2()
    {
        Debug.Log("Attack 2");
    }

    public void Combo3()
    {
        Debug.Log("Attack 3");
    }

    public void Combo4()
    {
        Debug.Log("Attack 4");
    }

    public void Combo5()
    {
        Debug.Log("Attack 5");
    }


    public static int CountMatches(List<int> required, List<int> taken)
    {
        int numMatches = 0;
        int index = 0;

        while (index < required.Count && index < taken.Count)
        {
            if (required[index] == taken[index])
            {
                numMatches++;
                index++;
            }
            else
            {
                break;
            }
        }

        return numMatches;
    }
}
