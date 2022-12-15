using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public Rigidbody2D rb;
    public Animator animator;
    public Transform center;
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
    public Portal currentPortal;

    public float currentAngle;

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
        //Calculate Angle
        currentAngle = CalculateAngle();
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
            SetCurrentLevel(currentLevelIndex);
        }
    }

    public void Move(int index)
    {
        if (InputHandler.Instance.canMove)
        {
            if (!collide)
            {
                currentAngle += + movementSpeed * Mathf.Rad2Deg * index * Time.deltaTime;
                //angle += movementSpeed * index * Time.deltaTime;
            }

            Vector3 targetPos = GetPosition(currentAngle, targetDistance);
            transform.position = center.position + targetPos + posOffset;

            //Vector3 offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
            //transform.position = offset + posOffset;
            //rb.MovePosition(offset + posOffset);
        }
    }

    //Get Next Position Based on Angle & Distance
    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    public void NextPlatform()
    {
        if (currentLevelIndex < levelList.Count - 1)
        {
            if (canGo && currentPortal != null)
            {
                currentPortal.TriggerPortal();
                StartCoroutine(CanMove());
            }

            if (currentLevelIndex == WinLayer)
            {
                StartCoroutine(Win());
            }

            
        }
        currentPortal = null;
    }

    public void SetCurrentLevel(int index)
    {
        var lastArena = GetComponentInParent<ArenaController>().active = false;
        transform.SetParent(levelList[index]);
        var currentArena = GetComponentInParent<ArenaController>().active = true;
        center = transform.parent;
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = center.position - transform.position;
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
        var relativePos = center.transform.position - transform.position;

        //Determine Angle
        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward);
        var direction2 = Vector3.Cross(forward, relativePos).x;
        return direction2;
    }

    public float CalculateAngle()
    {
        Vector2 dir = new Vector3(0, transform.position.y, 0) - transform.position;
        float result = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Portal>() != null)
        {
            //canGo = true;
            currentPortal = collision.GetComponent<Portal>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Portal")
        {
            //canGo = false;
            currentPortal = null;
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
