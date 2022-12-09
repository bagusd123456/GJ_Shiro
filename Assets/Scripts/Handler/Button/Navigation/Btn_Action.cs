using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_Action : MonoBehaviour,IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
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
        Debug.Log("Click Count: " + clickCount);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(btnSlider.value == 1)
        {
            btnSlider.value = 0;

            if (player != null)
                player.BasicAttack();
            else
                Debug.LogWarning("PlayerAttack Script Not Found");
            //Debug.Log("Attack");
        }
        else if(btnSlider.value == -1)
        {
            btnSlider.value = 0;
            StartCoroutine(player.Defend());
        }
        else
        {
            btnSlider.value = 0;
            //Debug.Log("Do Nothing");
        }
    }

    private void Awake()
    {
        if (btnSlider == null)
            btnSlider = GetComponent<Slider>();
        player = FindObjectOfType<PlayerAttack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
