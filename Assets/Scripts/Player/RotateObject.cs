using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    public Transform rotateAround;
    public float movementSpeed = 1f;
    public float targetDistance = 3f;
    public float angle;
    Vector3 offset;

    public Rigidbody2D rb;
    public List<Transform> levelList = new List<Transform>();
    public int currentLevelIndex = 0;
    public Transform player;

    public Vector3 currentPos;
    public static RotateObject Instance { get; private set; }

    //Mobile
    public int isMoving = 0;
    public bool Dead = false;


    public bool canMove = true;

    public int WinLayer;

    public Animator animator;

    public GameObject model;
    public float targetRotation;

    //Wall Detect
    bool collide;
    public float wallDistance = 1f;

    //Portal
    public bool canGo = false;

    public bool buswayPortal;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.localPosition);

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        collide = Physics2D.Raycast(transform.position, transform.up, wallDistance, LayerMask.GetMask("Wall"));

        if (!GetComponent<PlayerCharacter>().isDead)
        {
            //float vertical = Input.GetAxisRaw("Vertical");
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
            //SetCurrentLevel(currentLevelIndex);
            Move();
            LookAtTarget();

            
        }
    }

    public void SetCurrentLevel(int index)
    {
        transform.SetParent(levelList[index]);
    }

    public void Move()
    {
        if (canMove)
        {
            /*
            if (isMoving == 1 && !batasKanan)
            {
                angle += movementSpeed * 2 * Time.deltaTime;
            }
            if (isMoving == 2 && !batasKiri)
            {
                angle += movementSpeed * -2 * Time.deltaTime;
            }
            */
            if (!collide)
            {

                if (isMoving == 1 )
                {
                    angle += movementSpeed * 2 * Time.deltaTime;
                }
                if (isMoving == 2 )
                {
                    angle += movementSpeed * -2 * Time.deltaTime;
                }
            }
            //float horizontal = Input.GetAxisRaw("Horizontal");


            //targetDistance += -vertical * Time.deltaTime;
            if (offset != null)
            {
                offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
            }
            rb.MovePosition(offset); //rigidbody
                                     //transform.position = rotateAround.position + offset; //transform position
        }
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = rotateAround.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        if(isMoving == 1)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle,2f));
        else if( isMoving == 2)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle - 180, 2f));
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawLine(transform.position, rotateAround.position);

        if (collide)
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.red);
        else
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.white);
    }
    public void TurnLeft()
    {
        isMoving = 2;

        animator.SetBool("Running", true);
    }
    public void TurnRight()
    {
        isMoving = 1;

        animator.SetBool("Running", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Portal"))
        {
            canGo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Portal"))
        {
            canGo = false;
            buswayPortal = false;
        }
    }
    public void GoUp()
    {
        if (currentLevelIndex < levelList.Count - 1)
        {
            if (canGo)
            {
                Vector3 lastPos = transform.localPosition;
                currentLevelIndex++;
                transform.DOLocalMove(new Vector3(lastPos.x, 0.15f, lastPos.z), 1.5f);
                StartCoroutine(CanMove());
            }

            if (currentLevelIndex == WinLayer)
            {
                StartCoroutine(Win());
            }
            SetCurrentLevel(currentLevelIndex);
        }

        if (buswayPortal)
        {
            SceneLoad.Instance.panelBusway.SetActive(true);

            Time.timeScale = 0f;
        }
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoad.Instance.panelMenang.SetActive(true);
    }
    IEnumerator CanMove()
    {
        canMove = false;
        yield return new WaitForSeconds(1.5f);
        canMove = true;
    }
    public void Idle()
    {
        isMoving = 0;

        animator.SetBool("Running", false);
    }

}
