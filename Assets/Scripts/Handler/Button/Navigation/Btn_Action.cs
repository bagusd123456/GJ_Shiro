using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_Action : MonoBehaviour,IPointerClickHandler, IPointerUpHandler
{
    Slider btnSlider;
    PlayerAttack player;

    int clickCount;
    float clickTimer;
    public float timeBetweenClick = .5f;
    public void OnPointerClick(PointerEventData eventData)
    {
        clickTimer = timeBetweenClick;
        clickCount++;
        if (clickCount == 2)
            InputHandler.Instance.GoUp();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (InputHandler.Instance.canMove)
        {
            if (btnSlider.value == 1)
            {
                //Reset Value to 0
                btnSlider.value = 0;

                /*
                if (player != null)
                    player.RegisterCombo(1);
                */
                player.BasicAttack();

            }

            else if (btnSlider.value == -1)
            {
                if (player.currentComboTime >= 0)
                {
                    //Reset Value to 0
                    btnSlider.value = 0;
                    //Register Current Input to PlayerAttack
                    //player.RegisterCombo(-1);

                    //Trigger Defend Mechanic on First Combo
                    //if (player.comboIndex == 1)

                        StartCoroutine(player.Defend());

                }
            }

            else
                btnSlider.value = 0;
        }

        else
            btnSlider.value = 0;
    }

    private void Awake()
    {
        if (btnSlider == null)
            btnSlider = GetComponent<Slider>();
        player = FindObjectOfType<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickTimer > 0)
            clickTimer -= Time.deltaTime;
        else
            clickCount = 0;
    }
}
