using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillLaser : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject laserPrefab;

    public float scanInterval = 0.5f;
    float scanTime;

    public Transform rotateAround;
    public float targetDistance = 3;
    float targetAngle;
    Vector3 offset;
    private void Awake()
    {
        player = GetComponent<Boss_Kuro>().player.GetComponent<PlayerMovement>();
    }

    private void OnDisable()
    {
        laserPrefab.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        scanTime = scanInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (scanTime > 0)
        {
            scanTime -= Time.deltaTime;
        }

        else
        {
            scanTime = scanInterval;
            GetAngle();
            rotateAround.transform.localRotation = Quaternion.Euler(0, targetAngle, targetAngle);
            laserPrefab.SetActive(true);
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
