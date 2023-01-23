using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KuroLaser : MonoBehaviour
{
    public PlayerMovement player;
    public LaserDamage laserPrefab;
    [HideInInspector]
    public bool canShoot;

    public float scanInterval = 0.5f;
    public float scanTime;

    public Transform rotateAround;
    public float targetDistance = 3;
    Vector3 defaultRotation;
    float targetAngle;
    Vector3 offset;
    private void Awake()
    {
        player = GetComponent<Boss_Kuro>().player.GetComponent<PlayerMovement>();
        scanTime = scanInterval;
        defaultRotation = transform.eulerAngles;
    }

    private void OnDisable()
    {
        laserPrefab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (scanTime > 0)
        {
            scanTime -= Time.deltaTime;
            GetAngle();
            rotateAround.transform.localRotation = Quaternion.Euler(90, targetAngle - 90f, 0);
        }

        else
        {
            laserPrefab.gameObject.SetActive(true);
        }
    }

    public void GetAngle()
    {
        targetAngle = player.currentAngle;
    }

    public void SetPosition()
    {
        offset = new Vector3(Mathf.Sin(targetAngle) * targetDistance, Mathf.Cos(targetAngle) * targetDistance, 0) * targetDistance;
        transform.position = rotateAround.position + offset;
    }

    private void OnDrawGizmos()
    {
        
    }
}
