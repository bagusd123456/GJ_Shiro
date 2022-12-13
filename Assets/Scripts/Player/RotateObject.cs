using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    public static RotateObject Instance { get; private set; }
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject model;
    public Transform rotateAround;
    
    public float movementSpeed = 1f;
    public float targetDistance = 3f;
    public float angle;
    public enum rotateDir { LEFT, RIGHT }
    public rotateDir _rotateDir = rotateDir.RIGHT;

    public List<Transform> levelList = new List<Transform>();
    public int currentLevelIndex = 0;

    //Mobile
    public bool canMove = true;
    public int isMoving = 0;

    public int WinLayer;

    //Wall Detect
    bool collide;
    public float wallDistance = 1f;

    //Portal
    bool canGo = false;
    public bool buswayPortal;

    private void Awake()
    {
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

            Vector3 offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
            rb.MovePosition(offset);
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
        if (collide)
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.red);
        else
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.white);
    }
    public void TurnLeft()
    {
        isMoving = 2;
        _rotateDir = rotateDir.LEFT;
        animator.SetBool("Running", true);
    }
    public void TurnRight()
    {
        isMoving = 1;
        _rotateDir = rotateDir.RIGHT;

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
