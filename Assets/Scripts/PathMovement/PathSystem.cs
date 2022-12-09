using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PathSystem : MonoBehaviour
{
    public Transform player;
    public int targetIndex;
    public PathType pathType = PathType.CatmullRom;

    public GameObject[] targetObj = new GameObject[4];
    public List<Vector3> paths = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < targetIndex; i++)
        {

        }

        for (int i = 0; i < targetObj.Length; i++)
        {
            if(targetObj[i] != null)
                paths[i] = targetObj[i].transform.position;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(targetIndex>=0 && targetIndex <= targetObj.Length)
                targetIndex++;

            player.transform.DOMove(paths[targetIndex], 2);
            //player.transform.DOPath(paths.ToArray(), 2);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (targetIndex >= 0 && targetIndex <= targetObj.Length)
                targetIndex--;

            player.transform.DOMove(paths[targetIndex], 2);
        }
    }
}
