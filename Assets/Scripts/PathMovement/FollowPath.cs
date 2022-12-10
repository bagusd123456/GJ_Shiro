#define FollowPathv1
//#define FollowPathv2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.Rendering.DebugUI;

public class FollowPath : MonoBehaviour
{
    
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
        //Check Move Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        _state = horizontal > 0 || horizontal < 0 ? State.MOVING : State.IDLE;
        
        //Rotate Character to Mid
        Vector2 lookDir = pathArray[pathIndex].transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
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

        if (CheckDirection() < 0)
            Move(input);
        else if (CheckDirection() == 0)
            Move(input);
        else
        {
            Move(input);
        }

        direction = CheckDirection();
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
}
