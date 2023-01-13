using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool useGradient = false;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public CharacterBase player;
    public void Awake()
    {
        if(player == null)
            player = FindObjectOfType<PlayerCondition>();

        SetMaxHealth(player.statsData.HP);
    }

    public void Update()
    {
        SetHealth(player.currentHP);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        if(useGradient)
            fill.color = gradient.Evaluate(Time.deltaTime);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        if (useGradient)
            fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
