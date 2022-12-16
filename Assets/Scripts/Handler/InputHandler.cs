using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static PlayerMovement;

public class InputHandler : MonoBehaviour
{
    PlayerMovement player;

    public static InputHandler Instance { get; private set; }
    public int isMoving = 0;
    public bool invertInput = false;

    public bool canMove = true;

    public float direction;

    public enum state { IDLE, CHOOSE, BUSY}
    [Space]
    public state _state = state.IDLE;
    public Transform selectedPortal;
    private void Awake()
    {
        if(player == null)
            player = FindObjectOfType<PlayerMovement>();
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard Input
        if (!player.GetComponent<PlayerCondition>().isDead)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                TurnRight();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                TurnLeft();
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) ||
                Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                Idle();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                GoUp();
            }

            //Mobile Input
            MoveHandler();
        }

        direction = player.CheckDirection();
        
        if(_state == state.CHOOSE)
        {
            //ArenaManager.Instance.controllerList[+1].ToggleSelectablePortal(true);
        }
    }

    #region MobileInput

    public void MoveHandler()
    {
        if (isMoving == 1)
            player.Move(2);
        else if (isMoving == 2)
            player.Move(-2);
    }

    public void TurnLeft()
    {
        if (!invertInput)
        {
            isMoving = 2;
            player._rotateDir = rotateDir.LEFT;
        }
        else
        {
            isMoving = 1;
            player._rotateDir = rotateDir.RIGHT;
        }
    }

    public void TurnRight()
    {
        if (!invertInput)
        {
            isMoving = 1;
            player._rotateDir = rotateDir.RIGHT;
        }
        else
        {
            isMoving = 2;
            player._rotateDir = rotateDir.LEFT;
        }
    }

    public void Idle()
    {
        isMoving = 0;
    }

    public void GoUp()
    {
        player.NextPlatform();
    }
    #endregion

    public void InvertInput()
    {
        if (player.CheckDirection() > 0)
            invertInput = false;
        else if (player.CheckDirection() > 0.1f && player.CheckDirection() < 0.1f)
            invertInput = true;
        else
            invertInput = true;
    }

    public Transform GetSelectedPortal()
    {
        if(selectedPortal != null)
            return selectedPortal;
        else
            return null;
    }
}
