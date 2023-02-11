using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    public Transform center;
    Vector3 posOffset;
    public static PlayerMovement Instance { get; private set; }

    [Header("Movement Parameter")]
    public float movementSpeed = 1f;
    public float targetDistance = 2.8f;
    float angle;

    public enum rotateDir { LEFT, RIGHT }
    public rotateDir _rotateDir = rotateDir.RIGHT;

    public List<ArenaController> levelList = new List<ArenaController>();
    public int currentLevelIndex = 0;

    //Wall Detect
    public bool collide;
    public float wallDistance = 1f;

    //Portal
    public bool canGo = false;
    public Portal currentPortal;

    public float currentAngle;
    public Vector3 targetPos;
    public GameObject Shadow;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        //levelList = FindObjectsOfType<ArenaController>().OrderBy(x=> x.transform.GetSiblingIndex()).ToList();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        DOTween.Clear(true);
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
        collide = Physics.Raycast(transform.position, transform.up, wallDistance, LayerMask.GetMask("Wall"));
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
                currentAngle += movementSpeed * Mathf.Rad2Deg * index * Time.deltaTime;
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
                animator.SetTrigger("StartTP");
                currentPortal.TriggerPortal();
                StartCoroutine(CanMove());
            }

            if (currentLevelIndex == levelList.Count - 1)
            {
                Shadow.SetActive(false);
                StartCoroutine(Win());
            }
        }
        currentPortal = null;
    }

    public void SetCurrentLevel(int index)
    {
        var lastArena = GetComponentInParent<ArenaController>().active = false;
        transform.SetParent(levelList[index].transform);
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
        yield return new WaitForSeconds(2.5f);
        //SceneLoad.Instance.panelMenang.SetActive(true);
        SceneLoad.Instance.NextLevel();
    }

    public IEnumerator CanMove()
    {
        Shadow.SetActive(false);
        InputHandler.Instance.canMove = false;
        yield return new WaitForSeconds(1.5f);
        InputHandler.Instance.canMove = true;
        animator.SetTrigger("EndTP");

        if (currentLevelIndex != levelList.Count - 1)
            Shadow.SetActive(true);
    }

    public float CheckDirection()
    {
        var relativePos = center.transform.position - transform.position;

        //Determine Angle
        var forward = transform.forward;
        var direction2 = Vector3.Cross(forward, relativePos).x;
        return direction2;
    }

    public float CalculateAngle()
    {
        Vector3 dir = new Vector3(0, 0, transform.position.z) - transform.position;
        float result = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        if (result < 0)
            result = result * -1;
        return result;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Portal>() != null)
        {
            //canGo = true;
            currentPortal = collision.GetComponent<Portal>();
        }
    }

    private void OnTriggerExit(Collider collision)
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
