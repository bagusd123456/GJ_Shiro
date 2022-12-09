using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    public int isMove = 0;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove == 1)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (isMove == 2)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    public void SetMove(int index)
    {
        isMove = index;
    }
}
