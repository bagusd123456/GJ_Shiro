using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PathDoor : MonoBehaviour
{
    public enum DoorType { In,Out };
    public DoorType _doorType;
    public EndOfPathInstruction end;
    public float speed;
    public int _doorIndex;


    float dstTravelled;
    List<Transform> _transform;
    PathCreator _creator;
    private void Awake()
    {
        _creator = GetComponent<PathCreator>();

        
    }
    private void FixedUpdate()
    {
        _transform = new List<Transform>(GetComponentsInChildren<Transform>());
        _transform.Remove(transform);
    }

    private void Update()
    {
        
        switch (_doorType)
        {
            case DoorType.In:
                foreach (var item in _transform)
                {
                    if (item.position != _creator.path.GetPoint(_creator.path.localPoints.Length-1))
                    {
                        dstTravelled += speed * Time.deltaTime;
                        item.position = _creator.path.GetPointAtDistance(dstTravelled, end);
                    }
                }
                break;
            case DoorType.Out:
                foreach (var item in _transform)
                {
                    if (item.position != _creator.path.GetPoint(0))
                    {
                        dstTravelled -= speed * Time.deltaTime;
                        item.position = _creator.path.GetPointAtDistance(dstTravelled, end);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FollowPath player = collision.GetComponent<FollowPath>();
            player.canEnter = true;
            player.targetPath = _doorIndex;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FollowPath player = collision.GetComponent<FollowPath>();
            player.canEnter = false;
            player.targetPath = _doorIndex;
        }
    }
}
