using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBarStack : MonoBehaviour
{
    public List<Slider> healthBarList = new List<Slider>();
    public int healthValue;
    public float healthSpeed;

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
        //transform.rotation = camTransform.rotation * originalRotation;

        /*if(!player.isDead)
            healthBar.value = player.currentHP;*/
        HealthBarUpdate();
    }

    [ContextMenu("Shake HP")]
    public void ShakeHP()
    {
        healthValue -= 50;
        healthBarList[3].transform.Find("Fill Area").transform.DOShakePosition(0.5f,2,15);
    }

    public void HealthBarUpdate()
    {
        healthBarList[3].value = math.remap(750f, 1000f, 0f, 1f, healthValue);
        AnimateHP(3);
        healthBarList[2].value = math.remap(500f, 750f, 0f, 1f, healthValue);
        AnimateHP(2);
        healthBarList[1].value = math.remap(250f, 500f, 0f, 1f, healthValue);
        AnimateHP(1);
        healthBarList[0].value = math.remap(0f, 250f, 0f, 1f, healthValue);
        AnimateHP(0);
    }

    public void AnimateHP(int index)
    {
        Slider healthSlider = healthBarList[index].transform.Find("Slider_BG").GetComponentInChildren<Slider>();
        if (healthSlider.value >= healthBarList[index].value)
        {
            healthSlider.value -= healthSpeed;
        }
        else
            healthSlider.value = healthBarList[index].value;
    }
    public void CreateSlider(GameObject sliderPrefab,int quantity)
    {
        for (int i = healthBarList.Count; i < quantity; i++)
        {
            var hpSlider = Instantiate(sliderPrefab, this.gameObject.transform);
            healthBarList.Add(hpSlider.GetComponent<Slider>());
        }
    }

    public Slider HealthBar(int index)
    {
        Slider slider = healthBarList[index];
        return slider;
    }
}
