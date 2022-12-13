using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public Rigidbody rb;
    public Animator animator;
    public Transform rotateAround;
    public Vector3 posOffset;

    [Header("Movement Parameter")]
    public float movementSpeed = 1f;
    public float targetDistance = 3f;
    public float angle;

    public enum rotateDir { LEFT, RIGHT }
    public rotateDir _rotateDir = rotateDir.RIGHT;

    public List<Transform> levelList = new List<Transform>();
    public int currentLevelIndex = 0;

    public int WinLayer;

    //Wall Detect
    public bool collide;
    public float wallDistance = 1f;

    //Portal
    public bool canGo = false;
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

        if (!GetComponent<PlayerCondition>().isDead)
        {
            //float vertical = Input.GetAxisRaw("Vertical");

            if (InputHandler.Instance.isMoving == 1)
                animator.SetBool("Running", true);
            else if (InputHandler.Instance.isMoving == 2)
                animator.SetBool("Running", true);
            else
                animator.SetBool("Running", false);

            LookAtTarget();
        }
    }

    public void Move(int index)
    {
        if (InputHandler.Instance.canMove)
        {
            if (!collide)
            {
                angle += movementSpeed * index * Time.deltaTime;
            }

            Vector3 offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, transform.position.z) * targetDistance;
            //transform.position = offset + posOffset;
            rb.MovePosition(offset + posOffset);
        }
    }

    public void NextPlatform()
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

    public void SetCurrentLevel(int index)
    {
        transform.SetParent(levelList[index]);
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = rotateAround.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        if(InputHandler.Instance.isMoving == 1)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle,2f));
        else if(InputHandler.Instance.isMoving == 2)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, angle - 180, 2f));
    }
    
    public IEnumerator Win()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoad.Instance.panelMenang.SetActive(true);
    }

    public IEnumerator CanMove()
    {
        InputHandler.Instance.canMove = false;
        yield return new WaitForSeconds(1.5f);
        InputHandler.Instance.canMove = true;
    }

    public float CheckDirection()
    {
        var relativePos = rotateAround.transform.position - transform.position;

        //Determine Angle
        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward);
        var direction2 = Vector3.Cross(forward, relativePos).x;
        return direction2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Portal")
        {
            canGo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Portal")
        {
            canGo = false;
            buswayPortal = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (collide)
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.red);
        else
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.white);
    }
}
