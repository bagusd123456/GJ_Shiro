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
    public RotateObject rotateObject;
    public Transform pivot;
    public float targetDistance;

    public Vector3 offset;
    public float moveSpeed = 1f;
    public bool inverseRotation = false;
    
    private void Awake()
    {
        if (pivot == null)
            pivot = GameObject.Find("Target").transform;

        if(rotateObject == null)
            rotateObject = GameObject.FindGameObjectWithTag("Player").GetComponent<RotateObject>();
    }

    // Update is called once per frame
    void Update()
    {
        RotationSet();
    }

    Vector3 GetPosition(float degrees, float dist)
    {
        float a = degrees * Mathf.PI / 180f;
        return new Vector3(Mathf.Sin(a) * dist, Mathf.Cos(a) * dist, 0);
    }

    public void RotationSet()
    {
        //Rotate GameObject by Angle
        if (!inverseRotation)
            currentAngle += moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;
        else
            currentAngle += -moveSpeed * Mathf.Rad2Deg * 2f * Time.deltaTime;

        Vector3 targetPos = GetPosition(currentAngle, targetDistance);
        transform.position = pivot.position + targetPos + offset;
    }

}
