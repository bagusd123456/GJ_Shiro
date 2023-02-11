using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerMovement player;
    public void DescendLevel()
    {
        //Get to Lower Ground
        Vector3 lastPos = player.targetPos;
        transform.parent.DOLocalMove(new Vector3(lastPos.x, 0, lastPos.z), 1.5f);
    }
}
