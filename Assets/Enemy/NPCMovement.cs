using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform rotateAround;
    public float movementSpeed = 1f;
    public float targetDistance = 3f;
    public float angle;
    Vector3 offset;
    public bool isFacingRight = false;

    public Rigidbody2D rb;
    float time;
    public float timeMovement;
    public int moveDir;
    
    // Start is called before the first frame update
    void Start()
    {
        Flip();
        time = timeMovement;
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
        time = timeMovement;
        angle += movementSpeed * direction * Time.deltaTime;
        
        if (offset != null)
        {
            offset = new Vector3(Mathf.Sin(angle) * targetDistance, Mathf.Cos(angle) * targetDistance, 0) * targetDistance;
        }
        rb.MovePosition(offset); //rigidbody
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = rotateAround.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        if(collision.CompareTag("Player") && RotateObject.Instance.currentLevelIndex == 7)
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
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;
    }
}
