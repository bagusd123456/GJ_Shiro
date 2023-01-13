using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Image health;

    [HideInInspector]
    public Transform camTransform;

    Quaternion originalRotation;
    CharacterBase player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;

        if(!player.isDead)
            healthBar.value = player.currentHP;

        if (healthBar.value > 80)
            health.color = Color.green;
        else if (healthBar.value > 60)
            health.color = Color.yellow;
        else if (healthBar.value > 30)
            health.color = new Color(255, 165, 0);
        else
            health.color = Color.red;
    }
}
