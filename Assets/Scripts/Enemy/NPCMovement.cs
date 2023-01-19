using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public enum rotateDir { LEFT, RIGHT }
    public rotateDir _rotateDir = rotateDir.RIGHT;

    public Transform rotateAround;
    [HideInInspector] public float currentMovementSpeed;
    public float normalMovementSpeed ;
    public float hostileMovementSpeed;
    public float dashMovementSpeed;
    public float targetDistance = 3f;
    public float angle;

    float time;
    public float timeMovement;
    public int moveDir;
    public Transform charMesh;
    private void Awake()
    {
        if (rotateAround == null)
            rotateAround = transform.parent;

        angle = CalculateAngle();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
            time -= Time.deltaTime;

        Move(moveDir);
        LookAtTarget();
    }

    public void Move(int direction)
    {
        angle += currentMovementSpeed * Mathf.Rad2Deg * direction * Time.deltaTime;

        Vector3 targetPos = GetPosition(angle, targetDistance);
        transform.position = rotateAround.position + targetPos;
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = rotateAround.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if(moveDir > 0)
            transform.rotation = Quaternion.Euler(0, 0, angle);
        else
            transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("NPCBorder"))
        {
            if (moveDir == 1)
            {
                moveDir = -1;
                Flip();
            }
            else
            {
                moveDir = 1;
                Flip();
            }
        }
        if(collision.CompareTag("Player") && PlayerMovement.Instance.currentLevelIndex == 7)
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(Kalah());
        }
    }

    IEnumerator Kalah()
    {
        yield return new WaitForSeconds(1.5f);
        SceneLoad.Instance.panelKalah.SetActive(true);
    }

    void Flip()
    {
        if(_rotateDir == 0)
        {
            _rotateDir = rotateDir.RIGHT;
            charMesh.transform.rotation = Quaternion.Euler(new Vector3(180, 0, charMesh.transform.rotation.z));
        }
        else if(_rotateDir == (rotateDir)1)
        {
            _rotateDir = rotateDir.LEFT;
            charMesh.transform.rotation = Quaternion.Euler(new Vector3(180, 0, charMesh.transform.rotation.z));
        }

    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    float CalculateAngle()
    {
        Vector3 dir = rotateAround.position - transform.position;
        float result = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;
        return result;
    }
}
