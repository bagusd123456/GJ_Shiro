#define FollowPathv1
//#define FollowPathv2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    public float wallDistance = 1f;
    bool collide;

    public int targetPoint
    {
        get 
        {
            if (_targetPoint < 0) 
                return 0;
            else
                return _targetPoint; 
        }
        set { _targetPoint = value; }
    }
    public int _targetPoint;

    public float target;

    public enum State { IDLE, BUSY, MOVING };
    public State _state = State.IDLE;
    public PathCreator[] pathArray;
    int pathIndex;
    public int targetPath;
    public bool canEnter;

    public PathCreator creator;
    public Transform rotateAround;
    public float speed;

    float dstTravelled;
    float lastDistance;

    public int input;
    public float direction;
    public bool invertInput;
    // Start is called before the first frame update
    void Start()
    {
        creator = pathArray[pathIndex];
        transform.position = creator.path.GetPoint(1);
    }

    public void MoveLeft()
    {
        input = -1;
    }

    public void MoveRight()
    {
        input = 1;
    }

    public void Idle()
    {
        input = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Check Collider Ray
        collide = Physics2D.Raycast(transform.position, transform.up, wallDistance, LayerMask.GetMask("Wall"));

        //Check Move Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        _state = horizontal > 0 || horizontal < 0 ? State.MOVING : State.IDLE;
        
        //Rotate Character to Mid
        //Vector2 lookDir = pathArray[pathIndex].transform.position - transform.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        //transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Input.GetKeyDown(KeyCode.W) && _state == 0 && canEnter)
        {
#if FollowPathv1
            creator.lastDistance = dstTravelled;
            pathIndex = targetPath;

            creator = pathArray[pathIndex];
            dstTravelled = creator.lastDistance;
            transform.parent = creator.transform;
            transform.position = creator.path.GetClosestPointOnPath(transform.position);
#elif FollowPathv2
            creator.lastDistance = targetPoint;
            pathIndex = targetPath;

            creator = pathArray[pathIndex];
            targetPoint = (int)creator.lastDistance;
            transform.parent = creator.transform;
            transform.position = creator.path.GetClosestPointOnPath(transform.position);
#endif
        }

        else if (_state == 0 || _state == (State)2)
        {
            Move(horizontal);
        }

        else if(_state == State.BUSY)
        {

        }

        
        if (!collide)
        {
            //Invert Input Controller
            if (!invertInput)
                Move(input);
            else
                Move(-input);
            direction = CheckDirection();
        }

        LookAtTarget();
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = rotateAround.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Vector3 targetRotation = Vector3.zero;

        if (invertInput)
        {
            if (input == 1)
                targetRotation = new Vector3(0, 0, angle);
            else if (input == -1)
                targetRotation = new Vector3(0, 0, angle - 180);

        }
        else
        {
            if (input == -1)
                targetRotation = new Vector3(0, 0, angle);
            else if (input == 1)
                targetRotation = new Vector3(0, 0, angle - 180);
        }

        transform.rotation = Quaternion.Euler(targetRotation);
        
    }

    //Call Invert OnPointerUp
    public void InvertInput()
    {
        if (CheckDirection() < 0)
            invertInput = false;
        else if (CheckDirection() > 0.2f && CheckDirection() < 0.2f)
            invertInput = true;
        else
            invertInput = true;
    }

    public void Move(float horizontal)
    {
#if FollowPathv1
        dstTravelled += horizontal * speed * Time.deltaTime;
        transform.position = creator.path.GetPointAtDistance(dstTravelled, creator.pathEnd);

#elif FollowPathv2
        
        if (direction < 0)
        {
            target = horizontal;
            //Debug.Log("Normal");
        }

        else if(direction == 0)
        {
            if (horizontal > 0)
                target = horizontal;
            else
                target = -horizontal;
        }

        else
        {
            target = -horizontal;
            //Debug.Log("Invert");
        }

        //Move Object to Points
        if (targetPoint < creator.path.localPoints.Length - 1)
            targetPoint += (int)horizontal;
        else
            if (horizontal < 0)
                targetPoint += (int)horizontal;
            else
                targetPoint = 0;

        if (targetPoint == 0)
            if (horizontal > 0)
                targetPoint += (int)horizontal;
            else
                targetPoint = creator.path.localPoints.Length - 1;

        transform.position = Vector3.Lerp(transform.position, creator.path.GetPoint(targetPoint), 1.5f);
#endif
    }

    public float CheckDirection()
    {
        var relativePos = creator.transform.position - transform.position;

        //Determine Angle
        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward);
        var direction2 = Vector3.Cross(forward, relativePos).x;
        return direction2;
    }

    private void OnDrawGizmos()
    {
        if (collide)
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.red);
        else
            Debug.DrawRay(transform.position, transform.up * wallDistance, Color.white);
    }
}
