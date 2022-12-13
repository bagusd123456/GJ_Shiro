using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float _currentAngle;
    public float currentAngle 
    { 
        get 
        {
            return _currentAngle;
        }
        set 
        {
            if (_currentAngle > 360)
                _currentAngle = 0;
            else if (_currentAngle < -360)
                _currentAngle = 0;
            else
                _currentAngle = value;
        }
    }
    public float targetDistance;

    public Vector3 offset;
    public float moveSpeed = 1f;
    public Transform pivot;
    private void Awake()
    {
        if (pivot == null)
            pivot = GameObject.Find("Target").transform;

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate GameObject by Angle
        currentAngle += moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;
        Vector3 targetPos = GetPosition(currentAngle, targetDistance);
        transform.position = pivot.position + targetPos + offset;
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

}
